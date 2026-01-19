namespace GenExcel.API.Contracts.Health;

public sealed record DbHealthResult(
    string Status,
    string? Provider = null,
    string? Message = null,
    string[]? PendingMigrations = null
);

public sealed record HealthResponse(
    string Status,
    DateTime Utc,
    int ElapsedMs,
    DbHealthResult Database
);
