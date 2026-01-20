using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Tickets.Dtos;
using GenExcel.Application.Tickets.Ports;
using GenExcel.Application.Tickets.Queries;
using GenExcel.Domain.Entities;
using GenExcel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GenExcel.Infrastructure.Persistence.Repositories;

public sealed class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _db;

    public TicketRepository(AppDbContext db)
    {
        _db = db;
    }

    private static IQueryable<Ticket> ApplySorting(
        IQueryable<Ticket> query,
        string sort,
        string dir)
    {
        var asc = !string.Equals(dir, "desc", StringComparison.OrdinalIgnoreCase);

        return sort switch
        {
            "TicketType" => asc ? query.OrderBy(t => t.TicketType) : query.OrderByDescending(t => t.TicketType),
            "Status" => asc ? query.OrderBy(t => t.Status) : query.OrderByDescending(t => t.Status),
            "CreateDate" => asc ? query.OrderBy(t => t.CreateDate) : query.OrderByDescending(t => t.CreateDate),
            "UpdateDate" => asc ? query.OrderBy(t => t.UpdateDate) : query.OrderByDescending(t => t.UpdateDate),
            _ => asc ? query.OrderBy(t => t.TicketId) : query.OrderByDescending(t => t.TicketId),
        };
    }

    public async Task<PagedResult<TicketListDto>> SearchAsync(SearchTicketsQuery q, CancellationToken ct)
    {
        var query = _db.Ticket.AsNoTracking().AsQueryable();

        // ===== filtros =====
        if (!string.IsNullOrWhiteSpace(q.TicketType))
        {
            // Use Like pra ficar bom no SQL (e permitir índices dependendo do padrão)
            var term = q.TicketType.Trim();
            query = query.Where(t => EF.Functions.Like(t.TicketType, $"%{term}%"));
        }

        if (q.Status is not null)
            query = query.Where(t => t.Status == q.Status.Value);

        var total = await query.CountAsync(ct);

        // ===== ordenação =====
        query = ApplySorting(query, q.Sort, q.Dir);

        // ===== paginação =====
        var page = q.Page < 1 ? 1 : q.Page;
        var pageSize = q.PageSize is < 1 or > 200 ? 20 : q.PageSize;

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TicketListDto(
                t.TicketId,
                t.TicketType,
                t.Description,
                t.Status,
                t.CreateDate,
                t.UpdateDate
            // Se você adicionar Ticket.FeeRate no futuro, inclua aqui também.
            ))
            .ToListAsync(ct);

        return new PagedResult<TicketListDto>(items, page, pageSize, total);
    }

    public Task<Ticket?> GetByIdAsync(int ticketId, CancellationToken ct)
        => _db.Ticket.FirstOrDefaultAsync(t => t.TicketId == ticketId, ct);

    public Task AddAsync(Ticket entity, CancellationToken ct)
        => _db.Ticket.AddAsync(entity, ct).AsTask();

    public void Update(Ticket entity)
        => _db.Ticket.Update(entity);

    public void Remove(Ticket entity)
        => _db.Ticket.Remove(entity);

    public Task<int> SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
