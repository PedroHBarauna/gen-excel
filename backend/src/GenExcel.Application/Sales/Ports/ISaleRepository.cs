using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Sales.Dtos;
using GenExcel.Application.Sales.Queries;
using GenExcel.Domain.Entities;

namespace GenExcel.Application.Sales.Ports;

public interface ISaleRepository
{
    Task<PagedResult<SaleListDto>> SearchAsync(SearchSalesQuery query, CancellationToken ct);
    Task<Sale?> GetByIdAsync(int saleId, CancellationToken ct);
    Task AddAsync(Sale entity, CancellationToken ct);
    void Update(Sale entity);
    Task<int> SaveChangesAsync(CancellationToken ct);
}
