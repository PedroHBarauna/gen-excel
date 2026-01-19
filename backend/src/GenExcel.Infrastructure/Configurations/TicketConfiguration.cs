using GenExcel.Domain.Entities;
using GenExcel.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenExcel.Infrastructure.Persistence.Configurations;

public sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> b)
    {
        b.ToTable("Tickets");
        b.HasKey(x => x.TicketId);

        b.Property(x => x.TicketType).HasMaxLength(100).IsRequired();
        b.HasIndex(x => x.TicketType).IsUnique();

        b.Property(x => x.Description).HasMaxLength(500);

        b.Property(x => x.Status)
         .HasConversion<string>()
         .HasMaxLength(20)
         .IsRequired();



        b.Property(x => x.CreateDate).HasDefaultValueSql("GETDATE()");
    }
}
