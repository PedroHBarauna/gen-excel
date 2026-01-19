using GenExcel.Domain.Enums;

namespace GenExcel.Application.Events.Queries;

public sealed record SearchEventsQuery(
    string? Search,
    string? City,
    EventStatus? Status,
    DateTime? FromUtc,
    DateTime? ToUtc,
    int Page = 1,
    int PageSize = 20,
    string Sort = "EventDateTime",
    string Dir = "asc"
);
