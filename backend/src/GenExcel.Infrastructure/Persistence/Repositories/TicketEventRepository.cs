using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Tickets.Dtos;
using GenExcel.Application.Tickets.Ports;
using GenExcel.Application.Tickets.Queries;
using GenExcel.Domain.Entities;
using GenExcel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GenExcel.Infrastructure.Persistence.Repositories;

public sealed class TicketEventRepository : ITicketEventRepository
{
    private readonly AppDbContext _db;

    public TicketEventRepository(AppDbContext db)
    {
        _db = db;
    }

    private static IQueryable<Domain.Entities.TicketEvent> ApplySorting(
    IQueryable<Domain.Entities.TicketEvent> query,
    string sort,
    string dir)
    {
        var asc = !string.Equals(dir, "desc", StringComparison.OrdinalIgnoreCase);

        return sort switch
        {
            "Price" => asc ? query.OrderBy(t => t.Price) : query.OrderByDescending(t => t.Price),
            "Status" => asc ? query.OrderBy(t => t.Status) : query.OrderByDescending(t => t.Status),
            "SaleEndDate" => asc ? query.OrderBy(t => t.SaleEndDate) : query.OrderByDescending(t => t.SaleEndDate),
            _ => asc ? query.OrderBy(t => t.SaleStartDate) : query.OrderByDescending(t => t.SaleStartDate),
        };
    }

    public async Task<PagedResult<TicketEventListDto>> SearchAsync(SearchTicketsEventQuery q, CancellationToken ct)
    {
        var query = _db.TicketEvent.AsNoTracking().AsQueryable();

        if (q.EventId is not null)
            query = query.Where(t => t.EventId == q.EventId.Value);

        if (q.TicketId is not null)
            query = query.Where(t => t.TicketId == q.TicketId.Value);

        if (q.Status is not null)
            query = query.Where(t => t.Status == q.Status.Value);

        if (q.MinPrice is not null)
            query = query.Where(t => t.Price >= q.MinPrice.Value);

        if (q.MaxPrice is not null)
            query = query.Where(t => t.Price <= q.MaxPrice.Value);

        if (q.OnSaleNow)
        {
            var now = DateTime.UtcNow;
            query = query.Where(t =>
                t.Status == TicketEventStatus.Available &&
                t.Available > 0 &&
                t.SaleStartDate <= now &&
                t.SaleEndDate >= now);
        }

        var total = await query.CountAsync(ct);

        query = ApplySorting(query, q.Sort, q.Dir);

        var page = q.Page < 1 ? 1 : q.Page;
        var pageSize = q.PageSize is < 1 or > 200 ? 20 : q.PageSize;

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TicketEventListDto(
                t.TicketEventId,
                t.TicketId,
                t.EventId,
                t.Name,
                t.Description,
                t.Price,
                t.Available,
                t.Sold,
                t.SaleStartDate,
                t.SaleEndDate,
                t.Status
            ))
            .ToListAsync(ct);

        return new PagedResult<TicketEventListDto>(items, page, pageSize, total);
    }

    public Task<TicketEvent?> GetByIdAsync(int ticketId, CancellationToken ct)
    => _db.TicketEvent.FirstOrDefaultAsync(t => t.TicketId == ticketId, ct);

    public Task AddAsync(TicketEvent entity, CancellationToken ct)
        => _db.TicketEvent.AddAsync(entity, ct).AsTask();

    public void Update(TicketEvent entity)
        => _db.TicketEvent.Update(entity);

    public void Remove(TicketEvent entity)
        => _db.TicketEvent.Remove(entity);

    public Task<int> SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
