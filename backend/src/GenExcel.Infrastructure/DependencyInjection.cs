using GenExcel.Application.Events.Ports;
using GenExcel.Application.Exports.Reports;
using GenExcel.Application.Sales.Ports;
using GenExcel.Application.Tickets.Ports;
using GenExcel.Infrastructure.Exports.Reports;
using GenExcel.Infrastructure.Persistence;
using GenExcel.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenExcel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(cs, sql =>
            {
                sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);

                sql.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);
            }));
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketEventRepository, TicketEventRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IReportService, ReportService>();

        return services;
    }
}
