using GenExcel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Event", t =>
        {
            t.HasCheckConstraint("CHK_Status", "Status IN ('Active', 'Cancelled', 'Finished')");
        });

        builder.HasKey(x => x.EventId);

        builder.HasIndex(x => x.EventDateTime)
            .HasDatabaseName("IX_Event_DateTime");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_Event_Status");

        builder.Property(x => x.EventName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.Status)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();
    }
}