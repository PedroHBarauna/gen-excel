using GenExcel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sale", t =>
        {
            t.HasCheckConstraint(
                "CHK_PaymentStatus",
                "PaymentStatus IN ('Pending', 'Approved', 'Cancelled')");
        });

        builder.HasKey(x => x.SaleId);

        builder.Property(x => x.TotalPrice)
               .HasPrecision(10, 2);


        builder.HasIndex(x => x.EventId)
            .HasDatabaseName("IX_Sales_EventId");

        builder.HasIndex(x => x.TicketEventId)
            .HasDatabaseName("IX_Sales_TicketId");

        builder.HasOne(v => v.TicketEvent)
            .WithMany(ti => ti.Sales)
            .HasForeignKey(v => v.TicketEventId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Property(x => x.PaymentStatus)
               .HasConversion<string>()
               .HasMaxLength(20)
               .IsRequired();
    }
}
