using GenExcel.Domain.Entities;
using GenExcel.Domain.Enums;
using GenExcel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GenExcel.Infrastructure.Persistence.Seed;

public static class Seeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        if (await db.Event.AnyAsync())
            return;

        var now = DateTime.UtcNow;

        var rockEvent = new Event
        {
            EventName = "Linkin Park",
            Description = "World Tour with new vocalist Emily Armstrong",
            EventDateTime = now.AddDays(20),
            City = "Sao Paulo",
            TotalCapacity = 3000,
            Status = EventStatus.Active,
            CreateDate = now
        };

        var raggatonEvent = new Event
        {
            EventName = "Bad Bunny",
            Description = "DeBÍ TiRAR MáS FOToS World Tour",
            EventDateTime = now.AddDays(45),
            City = "Sao Paulo",
            TotalCapacity = 5000,
            Status = EventStatus.Active,
            CreateDate = now
        };

        db.Event.AddRange(rockEvent, raggatonEvent);
        await db.SaveChangesAsync();

        var tickets = new List<Ticket>
        {
            new()
            {
                TicketType = "Cadeira Superior",
                Description = "Assento em arquibancada/nível superior.",
                Status = TicketCatalogStatus.Active,
                CreateDate = now
            },
            new()
            {
                TicketType = "Cadeira Inferior",
                Description = "Assento em arquibancada/nível inferior, mais próximo do palco.",
                Status = TicketCatalogStatus.Active,
                CreateDate = now
            },
            new()
            {
                TicketType = "Pista",
                Description = "Acesso à pista comum.",
                Status = TicketCatalogStatus.Active,
                CreateDate = now
            },
            new()
            {
                TicketType = "Pista Premium",
                Description = "Acesso à pista premium, área privilegiada.",
                Status = TicketCatalogStatus.Active,
                CreateDate = now
            },
            new()
            {
                TicketType = "VIP",
                Description = "Acesso VIP, benefícios e/ou área exclusiva.",
                Status = TicketCatalogStatus.Active,
                CreateDate = now
            }
        };

        db.Ticket.AddRange(tickets);
        await db.SaveChangesAsync();

        var pista = tickets.First(t => t.TicketType == "Pista");
        var pistaPremium = tickets.First(t => t.TicketType == "Pista Premium");
        var cadeiraSuperior = tickets.First(t => t.TicketType == "Cadeira Superior");
        var cadeiraInferior = tickets.First(t => t.TicketType == "Cadeira Inferior");
        var vip = tickets.First(t => t.TicketType == "VIP");

        var ticketEvents = new List<TicketEvent>
        {
            new TicketEvent
            {
                TicketId = pista.TicketId,
                Name = pista.TicketType,
                Description = pista.Description!,
                EventId = rockEvent.EventId,
                Price = 120.00m,
                Available = 2500,
                Sold = 0,
                SaleStartDate = now.AddDays(-1),
                SaleEndDate = now.AddDays(19),
                Status = TicketEventStatus.Available,
                CreateDate = now
            },
            new TicketEvent
            {
                TicketId = pistaPremium.TicketId,
                Name = pistaPremium.TicketType,
                Description = pistaPremium.Description!,
                EventId = rockEvent.EventId,
                Price = 350.00m,
                Available = 500,
                Sold = 0,
                SaleStartDate = now.AddDays(-1),
                SaleEndDate = now.AddDays(19),
                Status = TicketEventStatus.Available,
                CreateDate = now
            },
            new TicketEvent
            {
                TicketId = vip.TicketId,
                Name = vip.TicketType,
                Description = vip.Description!,
                EventId = raggatonEvent.EventId,
                Price = 700.00m,
                Available = 200,
                Sold = 0,
                SaleStartDate = now.AddDays(-1),
                SaleEndDate = now.AddDays(44),
                Status = TicketEventStatus.Available,
                CreateDate = now
            },
            new TicketEvent
            {
                TicketId = cadeiraInferior.TicketId,
                Name = cadeiraInferior.TicketType,
                Description = cadeiraInferior.Description!,
                EventId = raggatonEvent.EventId,
                Price = 250.00m,
                Available = 1000,
                Sold = 0,
                SaleStartDate = now.AddDays(-1),
                SaleEndDate = now.AddDays(44),
                Status = TicketEventStatus.Available,
                CreateDate = now
            },
            new TicketEvent
            {
                TicketId = cadeiraSuperior.TicketId,
                Name = cadeiraSuperior.TicketType,
                Description = cadeiraSuperior.Description!,
                EventId = raggatonEvent.EventId,
                Price = 150.00m,
                Available = 2300,
                Sold = 0,
                SaleStartDate = now.AddDays(-1),
                SaleEndDate = now.AddDays(44),
                Status = TicketEventStatus.Available,
                CreateDate = now
            },
            new TicketEvent
            {
                TicketId = pista.TicketId,
                Name = pista.TicketType,
                Description = pista.Description!,
                EventId = raggatonEvent.EventId,
                Price = 200.00m,
                Available = 1000,
                Sold = 0,
                SaleStartDate = now.AddDays(-1),
                SaleEndDate = now.AddDays(44),
                Status = TicketEventStatus.Available,
                CreateDate = now
            },
            new TicketEvent
            {
                TicketId = pistaPremium.TicketId,
                Name = pistaPremium.TicketType,
                Description = pistaPremium.Description!,
                EventId = raggatonEvent.EventId,
                Price = 470.00m,
                Available = 500,
                Sold = 0,
                SaleStartDate = now.AddDays(-1),
                SaleEndDate = now.AddDays(44),
                Status = TicketEventStatus.Available,
                CreateDate = now
            },

        };

        db.TicketEvent.AddRange(ticketEvents);
        await db.SaveChangesAsync();

        var vipTicket = ticketEvents.First(t => t.TicketId == 1);
        var standardTicket = ticketEvents.First(t => t.TicketId == 2);

        var sales = new List<Sale>
        {
            new Sale
            {
                TicketEventId = vipTicket.TicketId,
                EventId = raggatonEvent.EventId,
                PurchaserName = "Pedro Barauna",
                PurchaseEmail = "pedro@example.com",
                PurchaserCpf = "123.456.789-00",
                Amount = 2,
                TotalPrice = 2 * (vipTicket.Price * 1.20m),
                SaleDate = now.AddMinutes(-30),
                PaymentForm = "Pix",
                PaymentStatus = PaymentStatus.Approved
            },
            new Sale
            {
                TicketEventId = standardTicket.TicketId,
                EventId = rockEvent.EventId,
                PurchaserName = "Maria Silva",
                PurchaseEmail = "maria@example.com",
                PurchaserCpf = "987.654.321-00",
                Amount = 1,
                TotalPrice = 1 * (standardTicket.Price * 1.20m),
                SaleDate = now.AddMinutes(-10),
                PaymentForm = "CreditCard",
                PaymentStatus = PaymentStatus.Pending
            }
        };

        db.Sale.AddRange(sales);

        vipTicket.Sold += 2;
        vipTicket.Available -= 2;

        standardTicket.Sold += 1;
        standardTicket.Available -= 1;

        await db.SaveChangesAsync();
    }
}
