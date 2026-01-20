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
                FeeRate = 0.20m,
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
                FeeRate = 0.20m,
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
                EventId = rockEvent.EventId,
                Price = 600.00m,
                Available = 200,
                Sold = 0,
                FeeRate = 0.20m,
                SaleStartDate = now.AddDays(-1),
                SaleEndDate = now.AddDays(44),
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
                FeeRate = 0.20m,
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
                FeeRate = 0.20m,
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
                FeeRate = 0.20m,
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
                FeeRate = 0.20m,
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
                FeeRate = 0.20m,
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
                TotalPrice = (2 * vipTicket.Price) + (2 * (vipTicket.Price * vipTicket.FeeRate)),
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
                TotalPrice = (1 * vipTicket.Price) + (1 * (vipTicket.Price * vipTicket.FeeRate)),
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

        var extraEvents = new List<Event>
{
    new()
    {
        EventName = "Taylor Swift",
        Description = "The Eras Tour",
        EventDateTime = now.AddDays(65),
        City = "Sao Paulo",
        TotalCapacity = 60000,
        Status = EventStatus.Active,
        CreateDate = now
    },
    new()
    {
        EventName = "Coldplay",
        Description = "Music of the Spheres World Tour",
        EventDateTime = now.AddDays(80),
        City = "Sao Paulo",
        TotalCapacity = 65000,
        Status = EventStatus.Active,
        CreateDate = now
    },
    new()
    {
        EventName = "UFC Fight Night",
        Description = "Main Card + Prelims",
        EventDateTime = now.AddDays(95),
        City = "Sao Paulo",
        TotalCapacity = 18000,
        Status = EventStatus.Active,
        CreateDate = now
    },
    new()
    {
        EventName = "Final - Campeonato Paulista",
        Description = "Jogo decisivo",
        EventDateTime = now.AddDays(110),
        City = "Sao Paulo",
        TotalCapacity = 48000,
        Status = EventStatus.Active,
        CreateDate = now
    },
    new()
    {
        EventName = "Formula 1 - GP do Brasil",
        Description = "Interlagos - Race Weekend",
        EventDateTime = now.AddDays(140),
        City = "Sao Paulo",
        TotalCapacity = 70000,
        Status = EventStatus.Active,
        CreateDate = now
    }
};

        db.Event.AddRange(extraEvents);
        await db.SaveChangesAsync();

        // FeeRate padrão (caso não venha do banco)
        static decimal FR(decimal? v) => v ?? 0.20m;

        // Helper: cria TicketEvent “padrão” (mantém consistência)
        static TicketEvent NewTicketEvent(
            int eventId,
            Ticket ticket,
            decimal price,
            int available,
            DateTime saleStart,
            DateTime saleEnd,
            DateTime nowUtc,
            decimal feeRate = 0.20m)
        {
            return new TicketEvent
            {
                TicketId = ticket.TicketId,
                Name = ticket.TicketType,
                Description = ticket.Description ?? "",
                EventId = eventId,
                Price = price,
                FeeRate = feeRate,
                Available = available,
                Sold = 0,
                SaleStartDate = saleStart,
                SaleEndDate = saleEnd,
                Status = TicketEventStatus.Available,
                CreateDate = nowUtc
            };
        }

        var saleStartGlobal = now.AddDays(-1);

        var extraTicketEvents = new List<TicketEvent>();

        {
            var ev = extraEvents[0];
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, cadeiraSuperior, 190m, 15000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.20m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, cadeiraInferior, 290m, 12000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.20m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, pista, 420m, 25000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.20m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, pistaPremium, 680m, 6000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.20m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, vip, 1200m, 2000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.20m));
        }

        {
            var ev = extraEvents[1];
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, cadeiraSuperior, 180m, 18000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.18m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, cadeiraInferior, 260m, 15000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.18m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, pista, 380m, 24000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.18m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, pistaPremium, 620m, 6000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.18m));
        }

        {
            var ev = extraEvents[2];
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, cadeiraSuperior, 120m, 7000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.15m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, cadeiraInferior, 220m, 6000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.15m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, pistaPremium, 450m, 3000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.15m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, vip, 900m, 1000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.15m));
        }

        {
            var ev = extraEvents[3];
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, cadeiraSuperior, 80m, 20000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.10m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, cadeiraInferior, 140m, 15000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.10m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, pista, 200m, 8000, saleStartGlobal, ev.EventDateTime.AddDays(-1), now, 0.10m));
        }

        {
            var ev = extraEvents[4];
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, cadeiraSuperior, 350m, 25000, saleStartGlobal, ev.EventDateTime.AddDays(-2), now, 0.25m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, cadeiraInferior, 520m, 20000, saleStartGlobal, ev.EventDateTime.AddDays(-2), now, 0.25m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, pistaPremium, 1100m, 8000, saleStartGlobal, ev.EventDateTime.AddDays(-2), now, 0.25m));
            extraTicketEvents.Add(NewTicketEvent(ev.EventId, vip, 2500m, 2000, saleStartGlobal, ev.EventDateTime.AddDays(-2), now, 0.25m));
        }

        db.TicketEvent.AddRange(extraTicketEvents);
        await db.SaveChangesAsync();

        // Helper para criar vendas
        static Sale NewSale(
            TicketEvent te,
            int eventId,
            string name,
            string email,
            string cpf,
            int amount,
            string paymentForm,
            PaymentStatus status,
            DateTime saleDateUtc)
        {
            var feeRate = te.FeeRate;
            var total = amount * (te.Price * (1m + feeRate));

            return new Sale
            {
                TicketEventId = te.TicketEventId,
                EventId = eventId,
                PurchaserName = name,
                PurchaseEmail = email,
                PurchaserCpf = cpf,
                Amount = amount,
                TotalPrice = total,
                SaleDate = saleDateUtc,
                PaymentForm = paymentForm,
                PaymentStatus = status
            };
        }

        static void ApplyInventory(TicketEvent te, int amount, PaymentStatus status)
        {
            if (status is PaymentStatus.Pending or PaymentStatus.Approved)
            {
                te.Sold += amount;
                te.Available -= amount;
                if (te.Available < 0) te.Available = 0;
            }
        }

        static TicketEvent Pick(List<TicketEvent> list, int eventId, string ticketType) =>
            list.First(x => x.EventId == eventId && x.Name == ticketType);

        var extraSales = new List<Sale>();

        {
            var evId = extraEvents[0].EventId;

            var te1 = Pick(extraTicketEvents, evId, "VIP");
            var te2 = Pick(extraTicketEvents, evId, "Pista Premium");
            var te3 = Pick(extraTicketEvents, evId, "Pista");
            var te4 = Pick(extraTicketEvents, evId, "Cadeira Inferior");
            var te5 = Pick(extraTicketEvents, evId, "Cadeira Superior");

            extraSales.Add(NewSale(te1, evId, "Ana Costa", "ana@example.com", "111.222.333-44", 1, "CreditCard", PaymentStatus.Approved, now.AddMinutes(-50)));
            ApplyInventory(te1, 1, PaymentStatus.Approved);

            extraSales.Add(NewSale(te2, evId, "Bruno Lima", "bruno@example.com", "222.333.444-55", 2, "Pix", PaymentStatus.Pending, now.AddMinutes(-40)));
            ApplyInventory(te2, 2, PaymentStatus.Pending);

            extraSales.Add(NewSale(te3, evId, "Carla Souza", "carla@example.com", "333.444.555-66", 3, "DebitCard", PaymentStatus.Approved, now.AddMinutes(-35)));
            ApplyInventory(te3, 3, PaymentStatus.Approved);

            extraSales.Add(NewSale(te4, evId, "Diego Alves", "diego@example.com", "444.555.666-77", 2, "Pix", PaymentStatus.Cancelled, now.AddMinutes(-30)));

            extraSales.Add(NewSale(te5, evId, "Eduarda Nunes", "edu@example.com", "555.666.777-88", 4, "CreditCard", PaymentStatus.Pending, now.AddMinutes(-25)));
            ApplyInventory(te5, 4, PaymentStatus.Pending);

            extraSales.Add(NewSale(te3, evId, "Felipe Rocha", "felipe@example.com", "666.777.888-99", 1, "Pix", PaymentStatus.Approved, now.AddMinutes(-20)));
            ApplyInventory(te3, 1, PaymentStatus.Approved);

            extraSales.Add(NewSale(te4, evId, "Gabi Martins", "gabi@example.com", "777.888.999-00", 1, "CreditCard", PaymentStatus.Approved, now.AddMinutes(-15)));
            ApplyInventory(te4, 1, PaymentStatus.Approved);
        }

        {
            var evId = extraEvents[1].EventId;

            var te1 = Pick(extraTicketEvents, evId, "Pista");
            var te2 = Pick(extraTicketEvents, evId, "Pista Premium");
            var te3 = Pick(extraTicketEvents, evId, "Cadeira Inferior");
            var te4 = Pick(extraTicketEvents, evId, "Cadeira Superior");

            extraSales.Add(NewSale(te1, evId, "Henrique Dias", "henrique@example.com", "101.202.303-40", 2, "Pix", PaymentStatus.Pending, now.AddMinutes(-55)));
            ApplyInventory(te1, 2, PaymentStatus.Pending);

            extraSales.Add(NewSale(te2, evId, "Isabela Melo", "isa@example.com", "202.303.404-50", 1, "CreditCard", PaymentStatus.Approved, now.AddMinutes(-48)));
            ApplyInventory(te2, 1, PaymentStatus.Approved);

            extraSales.Add(NewSale(te3, evId, "Joao Pedro", "joao@example.com", "303.404.505-60", 3, "CreditCard", PaymentStatus.Cancelled, now.AddMinutes(-41)));

            extraSales.Add(NewSale(te4, evId, "Kelly Santos", "kelly@example.com", "404.505.606-70", 2, "Pix", PaymentStatus.Approved, now.AddMinutes(-38)));
            ApplyInventory(te4, 2, PaymentStatus.Approved);

            extraSales.Add(NewSale(te1, evId, "Lucas Moraes", "lucas@example.com", "505.606.707-80", 1, "DebitCard", PaymentStatus.Approved, now.AddMinutes(-33)));
            ApplyInventory(te1, 1, PaymentStatus.Approved);
        }

        {
            var evId = extraEvents[2].EventId;

            var te1 = Pick(extraTicketEvents, evId, "VIP");
            var te2 = Pick(extraTicketEvents, evId, "Cadeira Inferior");
            var te3 = Pick(extraTicketEvents, evId, "Cadeira Superior");

            extraSales.Add(NewSale(te2, evId, "Mariana Braga", "mariana@example.com", "606.707.808-90", 2, "Pix", PaymentStatus.Pending, now.AddMinutes(-60)));
            ApplyInventory(te2, 2, PaymentStatus.Pending);

            extraSales.Add(NewSale(te1, evId, "Nicolas Prado", "nicolas@example.com", "707.808.909-10", 1, "CreditCard", PaymentStatus.Approved, now.AddMinutes(-52)));
            ApplyInventory(te1, 1, PaymentStatus.Approved);

            extraSales.Add(NewSale(te3, evId, "Olivia Araujo", "olivia@example.com", "808.909.010-11", 3, "CreditCard", PaymentStatus.Cancelled, now.AddMinutes(-45)));
        }

        {
            var evId = extraEvents[3].EventId;

            var te1 = Pick(extraTicketEvents, evId, "Cadeira Superior");
            var te2 = Pick(extraTicketEvents, evId, "Cadeira Inferior");
            var te3 = Pick(extraTicketEvents, evId, "Pista");

            extraSales.Add(NewSale(te1, evId, "Paulo Vitor", "paulo@example.com", "909.010.111-22", 4, "Pix", PaymentStatus.Pending, now.AddMinutes(-70)));
            ApplyInventory(te1, 4, PaymentStatus.Pending);

            extraSales.Add(NewSale(te2, evId, "Rafaela Pires", "rafa@example.com", "010.111.222-33", 2, "DebitCard", PaymentStatus.Approved, now.AddMinutes(-62)));
            ApplyInventory(te2, 2, PaymentStatus.Approved);

            extraSales.Add(NewSale(te3, evId, "Sergio Lima", "sergio@example.com", "111.222.333-45", 1, "CreditCard", PaymentStatus.Approved, now.AddMinutes(-58)));
            ApplyInventory(te3, 1, PaymentStatus.Approved);

            extraSales.Add(NewSale(te2, evId, "Talita Gomes", "talita@example.com", "222.333.444-56", 2, "Pix", PaymentStatus.Cancelled, now.AddMinutes(-53)));
        }

        {
            var evId = extraEvents[4].EventId;

            var te1 = Pick(extraTicketEvents, evId, "VIP");
            var te2 = Pick(extraTicketEvents, evId, "Pista Premium");
            var te3 = Pick(extraTicketEvents, evId, "Cadeira Inferior");
            var te4 = Pick(extraTicketEvents, evId, "Cadeira Superior");

            extraSales.Add(NewSale(te4, evId, "Ursula Teixeira", "ursula@example.com", "333.444.555-67", 2, "Pix", PaymentStatus.Pending, now.AddMinutes(-90)));
            ApplyInventory(te4, 2, PaymentStatus.Pending);

            extraSales.Add(NewSale(te3, evId, "Victor Hugo", "victor@example.com", "444.555.666-78", 1, "CreditCard", PaymentStatus.Approved, now.AddMinutes(-84)));
            ApplyInventory(te3, 1, PaymentStatus.Approved);

            extraSales.Add(NewSale(te2, evId, "Wesley Barros", "wesley@example.com", "555.666.777-89", 2, "CreditCard", PaymentStatus.Pending, now.AddMinutes(-80)));
            ApplyInventory(te2, 2, PaymentStatus.Pending);

            extraSales.Add(NewSale(te1, evId, "Xuxa Silva", "xuxa@example.com", "666.777.888-90", 1, "Pix", PaymentStatus.Approved, now.AddMinutes(-73)));
            ApplyInventory(te1, 1, PaymentStatus.Approved);

            extraSales.Add(NewSale(te3, evId, "Yara Monteiro", "yara@example.com", "777.888.999-12", 3, "DebitCard", PaymentStatus.Cancelled, now.AddMinutes(-66)));

            extraSales.Add(NewSale(te4, evId, "Zeca Ribeiro", "zeca@example.com", "888.999.000-13", 1, "Pix", PaymentStatus.Approved, now.AddMinutes(-61)));
            ApplyInventory(te4, 1, PaymentStatus.Approved);
        }

        db.Sale.AddRange(extraSales);
        await db.SaveChangesAsync();
    }
}
