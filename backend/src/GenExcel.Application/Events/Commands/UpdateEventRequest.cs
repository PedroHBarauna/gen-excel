using GenExcel.Domain.Enums;

namespace GenExcel.Application.Events.Commands;

public sealed record UpdateEventRequest(
    string? Description,
    DateTime EventDateTime,
    string City,
    int TotalCapacity,
    EventStatus Status
);
