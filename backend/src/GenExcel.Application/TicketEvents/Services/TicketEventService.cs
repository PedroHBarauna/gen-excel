using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Tickets.Commands;
using GenExcel.Application.Tickets.Dtos;
using GenExcel.Application.Tickets.Ports;
using GenExcel.Application.Tickets.Queries;
using GenExcel.Domain.Entities;

namespace GenExcel.Application.Tickets.Services;

public sealed class TicketEventService : ITicketEventService
{
    private readonly ITicketEventRepository _repo;

    public TicketEventService(ITicketEventRepository queries)
    {
        _repo = queries;
    }

    public Task<PagedResult<TicketEventListDto>> SearchAsync(SearchTicketsEventQuery query, CancellationToken ct)
        => _repo.SearchAsync(query, ct);

    public async Task<TicketEventDetailsDto?> GetByIdAsync(int ticketId, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(ticketId, ct);
        return entity is null ? null : Map(entity);
    }

    public async Task<TicketEventDetailsDto> CreateAsync(CreateTicketEventRequest request, CancellationToken ct)
    {
        ValidateCreate(request);

        var now = DateTime.UtcNow;

        var entity = new TicketEvent
        {
            TicketId = request.TicketId,
            EventId = request.EventId,
            Price = request.Price,
            Available = request.Available,
            Sold = 0,
            SaleStartDate = request.SaleStartDate,
            SaleEndDate = request.SaleEndDate,
            Status = request.Status,
            CreateDate = now
        };

        await _repo.AddAsync(entity, ct);
        await _repo.SaveChangesAsync(ct);

        return Map(entity);
    }

    public async Task<TicketEventDetailsDto?> UpdateAsync(int ticketEventId, UpdateTicketEventRequest request, CancellationToken ct)
    {
        ValidateUpdate(request);

        var entity = await _repo.GetByIdAsync(ticketEventId, ct);
        if (entity is null) return null;

        
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;
        entity.Available = request.Available;
        entity.SaleStartDate = request.SaleStartDate;
        entity.SaleEndDate = request.SaleEndDate;
        entity.Status = request.Status;
        entity.UpdateDate = DateTime.UtcNow;

        _repo.Update(entity);
        await _repo.SaveChangesAsync(ct);

        return Map(entity);
    }

    public async Task<bool> DeleteAsync(int ticketId, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(ticketId, ct);
        if (entity is null) return false;

        _repo.Remove(entity);
        await _repo.SaveChangesAsync(ct);

        return true;
    }

    private static void ValidateCreate(CreateTicketEventRequest r)
    {
        if (r.EventId <= 0) throw new ArgumentException("EventId is required.");
        if (string.IsNullOrWhiteSpace(r.Name)) throw new ArgumentException("Name is required.");
        if (r.Price < 0) throw new ArgumentException("Price must be >= 0.");
        if (r.Available < 0) throw new ArgumentException("Available must be >= 0.");
        if (r.SaleEndDate < r.SaleStartDate) throw new ArgumentException("SaleEndDate must be >= SaleStartDate.");
    }

    private static void ValidateUpdate(UpdateTicketEventRequest r)
    {
        if (r.Price < 0) throw new ArgumentException("Price must be >= 0.");
        if (r.Available < 0) throw new ArgumentException("Available must be >= 0.");
        if (r.SaleEndDate < r.SaleStartDate) throw new ArgumentException("SaleEndDate must be >= SaleStartDate.");
    }

    private static TicketEventDetailsDto Map(TicketEvent t) =>
        new(
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
            t.Status,
            t.CreateDate,
            t.UpdateDate
        );
}
