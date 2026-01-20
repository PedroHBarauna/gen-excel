using GenExcel.Domain.Entities;
using GenExcel.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace GenExcel.Infrastructure.Persistence.Configurations;

public sealed class TicketEventConfiguration : IEntityTypeConfiguration<TicketEvent>
{
    public void Configure(EntityTypeBuilder<TicketEvent> b)
    {
        b.ToTable("TicketEvents");
        b.HasKey(x => x.TicketEventId);

        b.Property(x => x.Price).HasColumnType("decimal(10,2)").IsRequired();
        b.Property(x => x.FeeRate).HasColumnType("decimal(5,2)").IsRequired();
        b.Property(x => x.Available).IsRequired();
        b.Property(x => x.Sold).HasDefaultValue(0).IsRequired();

        b.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(TicketEventStatus.Available);

        b.Property(x => x.CreateDate).HasDefaultValueSql("GETDATE()");

        b.HasOne(x => x.Event)
            .WithMany(e => e.TicketEvents)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Ticket)
            .WithMany(t => t.TicketEvents)
            .HasForeignKey(x => x.TicketId)
            .OnDelete(DeleteBehavior.NoAction);


        b.HasIndex(x => new { x.EventId, x.TicketId }).IsUnique();
        b.HasIndex(x => x.EventId);
        b.HasIndex(x => x.Status);
    }
}
