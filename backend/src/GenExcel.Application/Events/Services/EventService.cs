using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Events.Commands;
using GenExcel.Application.Events.Dtos;
using GenExcel.Application.Events.Ports;
using GenExcel.Application.Events.Queries;
using GenExcel.Domain.Entities;

namespace GenExcel.Application.Events.Services;

public sealed class EventService : IEventService
{
    private readonly IEventRepository _repo;

    public EventService(IEventRepository repo)
    {
        _repo = repo;
    }

    public Task<PagedResult<EventListDto>> SearchAsync(SearchEventsQuery query, CancellationToken ct)
        => _repo.SearchAsync(query, ct);

    public async Task<EventDetailsDto> CreateAsync(CreateEventRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.EventName))
            throw new ArgumentException("EventName is required.");

        if (string.IsNullOrWhiteSpace(request.City))
            throw new ArgumentException("City is required.");

        if (request.TotalCapacity < 0)
            throw new ArgumentException("TotalCapacity must be >= 0.");

        var now = DateTime.UtcNow;

        var entity = new Event
        {
            EventName = request.EventName.Trim(),
            Description = request.Description,
            EventDateTime = request.EventDateTime,
            City = request.City.Trim(),
            TotalCapacity = request.TotalCapacity,
            Status = request.Status,
            CreateDate = now
        };

        await _repo.AddAsync(entity, ct);
        await _repo.SaveChangesAsync(ct);

        return Map(entity);
    }

    public async Task<EventDetailsDto?> GetByIdAsync(int eventId, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(eventId, ct);
        return entity is null ? null : Map(entity);
    }


    public async Task<EventDetailsDto?> UpdateAsync(int eventId, UpdateEventRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(eventId, ct);
        if (entity is null) return null;


        if (string.IsNullOrWhiteSpace(request.City))
            throw new ArgumentException("City is required.");

        if (request.TotalCapacity < 0)
            throw new ArgumentException("TotalCapacity must be more than 0");

        entity.Description = request.Description;
        entity.EventDateTime = request.EventDateTime;
        entity.City = request.City.Trim();
        entity.TotalCapacity = request.TotalCapacity;
        entity.Status = request.Status;
        entity.UpdateDate = DateTime.UtcNow;

        _repo.Update(entity);
        await _repo.SaveChangesAsync(ct);

        return Map(entity);
    }

    public async Task<bool> DeleteAsync(int eventId, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(eventId, ct);
        if (entity is null) return false;

        _repo.Remove(entity);
        await _repo.SaveChangesAsync(ct);

        return true;
    }

    private static EventDetailsDto Map(Event e) =>
        new(
            e.EventId,
            e.EventName,
            e.Description,
            e.EventDateTime,
            e.City,
            e.TotalCapacity,
            e.Status,
            e.CreateDate,
            e.UpdateDate
        );
}
