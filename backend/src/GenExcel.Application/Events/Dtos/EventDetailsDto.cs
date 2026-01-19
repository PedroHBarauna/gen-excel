using GenExcel.Domain.Enums;

namespace GenExcel.Application.Events.Dtos;

public sealed record EventDetailsDto(
    int EventId,
    string EventName,
    string? Description,
    DateTime EventDateTime,
    string City,
    int TotalCapacity,
    EventStatus Status,
    DateTime CreateDate,
    DateTime? UpdateDate
);
