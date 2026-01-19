using GenExcel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenExcel.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Event> Event => Set<Event>();
    public DbSet<Ticket> Ticket => Set<Ticket>();
    public DbSet<TicketEvent> TicketEvent => Set<TicketEvent>();
    public DbSet<Sale> Sale => Set<Sale>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
