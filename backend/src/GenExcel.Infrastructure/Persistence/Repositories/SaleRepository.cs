using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Sales.Dtos;
using GenExcel.Application.Sales.Ports;
using GenExcel.Application.Sales.Queries;
using GenExcel.Domain.Entities;
using GenExcel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GenExcel.Infrastructure.Persistence.Repositories;

public sealed class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _db;

    public SaleRepository(AppDbContext db)
    {
        _db = db;
    }

    private static IQueryable<Domain.Entities.Sale> ApplySorting(
    IQueryable<Domain.Entities.Sale> query,
    string sort,
    string dir)
    {
        var asc = !string.Equals(dir, "desc", StringComparison.OrdinalIgnoreCase);

        return sort switch
        {
            "TotalPrice" => asc ? query.OrderBy(s => s.TotalPrice) : query.OrderByDescending(s => s.TotalPrice),
            "Amount" => asc ? query.OrderBy(s => s.Amount) : query.OrderByDescending(s => s.Amount),
            "PaymentStatus" => asc ? query.OrderBy(s => s.PaymentStatus) : query.OrderByDescending(s => s.PaymentStatus),
            _ => asc ? query.OrderBy(s => s.SaleDate) : query.OrderByDescending(s => s.SaleDate),
        };
    }

    public async Task<PagedResult<SaleListDto>> SearchAsync(SearchSalesQuery q, CancellationToken ct)
    {
        var query = _db.Sale.AsNoTracking().AsQueryable();

        if (q.EventId is not null)
            query = query.Where(s => s.EventId == q.EventId.Value);

        if (q.TicketId is not null)
            query = query.Where(s => s.TicketEventId == q.TicketId.Value);

        if (q.PaymentStatus is not null)
            query = query.Where(s => s.PaymentStatus == q.PaymentStatus.Value);

        if (!string.IsNullOrWhiteSpace(q.Email))
        {
            var email = q.Email.Trim();
            query = query.Where(s => s.PurchaseEmail == email);
        }

        if (!string.IsNullOrWhiteSpace(q.Cpf))
        {
            var cpf = q.Cpf.Trim();
            query = query.Where(s => s.PurchaserCpf == cpf);
        }

        if (q.FromUtc is not null)
            query = query.Where(s => s.SaleDate >= q.FromUtc.Value);

        if (q.ToUtc is not null)
            query = query.Where(s => s.SaleDate <= q.ToUtc.Value);

        var total = await query.CountAsync(ct);

        query = ApplySorting(query, q.Sort, q.Dir);

        var page = q.Page < 1 ? 1 : q.Page;
        var pageSize = q.PageSize is < 1 or > 200 ? 20 : q.PageSize;

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new SaleListDto(
                s.SaleId,
                s.EventId,
                s.TicketEventId,
                s.PurchaserName,
                s.PurchaseEmail,
                s.Amount,
                s.TotalPrice,
                s.SaleDate,
                s.PaymentForm,
                s.PaymentStatus
            ))
            .ToListAsync(ct);

        return new PagedResult<SaleListDto>(items, page, pageSize, total);
    }

    public Task<Sale?> GetByIdAsync(int saleId, CancellationToken ct)
    => _db.Sale.FirstOrDefaultAsync(s => s.SaleId == saleId, ct);

    public Task AddAsync(Sale entity, CancellationToken ct)
        => _db.Sale.AddAsync(entity, ct).AsTask();

    public void Update(Sale entity)
        => _db.Sale.Update(entity);

    public Task<int> SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
