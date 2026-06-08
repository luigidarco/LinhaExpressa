using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LinhaExpressa.Application.Frota;
using LinhaExpressa.Infrastructure.Hubs;
using LinhaExpressa.Infrastructure.Persistence;
using LinhaExpressa.Infrastructure.Persistence.Repositories;

namespace LinhaExpressa.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var connection = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Conexão não configurada.");

        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(connection, sql => sql.EnableRetryOnFailure()));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IConfiguracaoGlobalProvider, ConfiguracaoGlobalProvider>();
        services.AddScoped<IOnibusRepository, OnibusRepository>();
        services.AddScoped<IOnibusService, OnibusService>();
        services.AddScoped<INotificationService, SignalRNotificationService>();
        services.AddSignalR();

        return services;
    }
}
