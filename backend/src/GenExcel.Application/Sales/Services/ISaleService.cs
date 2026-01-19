using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Sales.Commands;
using GenExcel.Application.Sales.Dtos;
using GenExcel.Application.Sales.Queries;

namespace GenExcel.Application.Sales.Services;

public interface ISaleService
{
    Task<PagedResult<SaleListDto>> SearchAsync(SearchSalesQuery query, CancellationToken ct);
    Task<SaleDetailsDto?> GetByIdAsync(int saleId, CancellationToken ct);
    Task<SaleDetailsDto> CreateAsync(CreateSaleRequest request, CancellationToken ct);
    Task<SaleDetailsDto?> UpdateAsync(int saleId, UpdateSaleRequest request, CancellationToken ct);
}
