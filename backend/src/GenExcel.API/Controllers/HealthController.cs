using GenExcel.API.Contracts.Health;
using GenExcel.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenExcel.API.Controllers;

[Authorize]
[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    private readonly AppDbContext _db;

    public HealthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<HealthResponse>> Get(CancellationToken ct)
    {
        var start = DateTime.UtcNow;

        var db = await CheckDatabaseAsync(ct);

        var overallStatus = db.Status == "Healthy" ? "Healthy" : "Unhealthy";

        var response = new HealthResponse(
            Status: overallStatus,
            Utc: DateTime.UtcNow,
            ElapsedMs: (int)(DateTime.UtcNow - start).TotalMilliseconds,
            Database: db
        );

        return Ok(response);
    }

    private async Task<DbHealthResult> CheckDatabaseAsync(CancellationToken ct)
    {
        try
        {
            var canConnect = await _db.Database.CanConnectAsync(ct);
            if (!canConnect)
                return new DbHealthResult(Status: "Unhealthy", Message: "Cannot connect to database.");

            var pending = await _db.Database.GetPendingMigrationsAsync(ct);

            return new DbHealthResult(
                Status: "Healthy",
                Provider: _db.Database.ProviderName,
                PendingMigrations: pending.ToArray()
            );
        }
        catch (OperationCanceledException)
        {
            return new DbHealthResult(Status: "Unhealthy", Message: "Health check cancelled.");
        }
        catch (Exception ex)
        {
            return new DbHealthResult(Status: "Unhealthy", Message: ex.Message);
        }
    }
}
