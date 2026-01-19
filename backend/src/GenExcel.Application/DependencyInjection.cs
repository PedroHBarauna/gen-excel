using GenExcel.Application.Events.Services;
using GenExcel.Application.Tickets.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GenExcel.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplications(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<ITicketEventService, TicketEventService>();
        return services;
    }
}
