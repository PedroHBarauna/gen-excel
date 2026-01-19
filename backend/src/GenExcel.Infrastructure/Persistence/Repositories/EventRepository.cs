using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Events.Dtos;
using GenExcel.Application.Events.Ports;
using GenExcel.Application.Events.Queries;
using GenExcel.Domain.Entities;
using GenExcel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GenExcel.Infrastructure.Persistence.Repositories;

public sealed class EventRepository : IEventRepository
{
    private readonly AppDbContext _db;

    public EventRepository(AppDbContext db)
    {
        _db = db;
    }

    private static IQueryable<Domain.Entities.Event> ApplySorting(
    IQueryable<Domain.Entities.Event> query,
    string sort,
    string dir)
    {
        var asc = !string.Equals(dir, "desc", StringComparison.OrdinalIgnoreCase);

        return sort switch
        {
            "EventName" => asc ? query.OrderBy(e => e.EventName) : query.OrderByDescending(e => e.EventName),
            "City" => asc ? query.OrderBy(e => e.City) : query.OrderByDescending(e => e.City),
            "Status" => asc ? query.OrderBy(e => e.Status) : query.OrderByDescending(e => e.Status),
            _ => asc ? query.OrderBy(e => e.EventDateTime) : query.OrderByDescending(e => e.EventDateTime),
        };
    }
    public async Task<PagedResult<EventListDto>> SearchAsync(SearchEventsQuery q, CancellationToken ct)
    {
        var query = _db.Event.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(q.Search))
        {
            var s = q.Search.Trim();
            query = query.Where(e =>
                EF.Functions.Like(e.EventName, $"%{s}%") ||
                (e.Description != null && EF.Functions.Like(e.Description, $"%{s}%")));
        }

        if (!string.IsNullOrWhiteSpace(q.City))
            query = query.Where(e => e.City == q.City);

        if (q.Status is not null)
            query = query.Where(e => e.Status == q.Status.Value);

        if (q.FromUtc is not null)
            query = query.Where(e => e.EventDateTime >= q.FromUtc.Value);

        if (q.ToUtc is not null)
            query = query.Where(e => e.EventDateTime <= q.ToUtc.Value);

        var total = await query.CountAsync(ct);

        query = ApplySorting(query, q.Sort, q.Dir);

        var page = q.Page < 1 ? 1 : q.Page;
        var pageSize = q.PageSize is < 1 or > 200 ? 20 : q.PageSize;

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EventListDto(
                e.EventId,
                e.EventName,
                e.Description,
                e.EventDateTime,
                e.City,
                e.TotalCapacity,
                e.Status
            ))
            .ToListAsync(ct);

        return new PagedResult<EventListDto>(items, page, pageSize, total);
    }

    public Task<Event?> GetByIdAsync(int eventId, CancellationToken ct)
    => _db.Event.FirstOrDefaultAsync(e => e.EventId == eventId, ct);

    public Task AddAsync(Event entity, CancellationToken ct)
        => _db.Event.AddAsync(entity, ct).AsTask();

    public void Update(Event entity)
        => _db.Event.Update(entity);

    public void Remove(Event entity)
        => _db.Event.Remove(entity);

    public Task<int> SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
