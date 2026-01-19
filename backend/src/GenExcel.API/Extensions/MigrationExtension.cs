using GenExcel.Infrastructure.Persistence;
using GenExcel.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace GenExcel.API.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var asm = db.GetType().Assembly.FullName;
        Console.WriteLine($">>> DbContext assembly: {asm}");

        var applied = db.Database.GetAppliedMigrations().ToList();
        var pending = db.Database.GetPendingMigrations().ToList();

        Console.WriteLine($">>> Applied: {applied.Count}");
        foreach (var m in applied) Console.WriteLine($">>>   [A] {m}");

        Console.WriteLine($">>> Pending: {pending.Count}");
        foreach (var m in pending) Console.WriteLine($">>>   [P] {m}");

        var retries = 5;
        while (retries > 0)
        {
            try
            {
                db.Database.Migrate();
                Seeder.SeedAsync(db).GetAwaiter().GetResult();
                return;
            }
            catch(Exception ex)
            {
                retries--;
                Console.WriteLine($">>> Migrate failed: {ex.Message}");
                Thread.Sleep(3000);
            }
        }

        throw new Exception("Não foi possível aplicar migrations.");
    }

}
