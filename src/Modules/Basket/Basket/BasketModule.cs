using Basket.Data.Processors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Shared.Data.Interceptors;

namespace Basket;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        #region Infrastructure services
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.Decorate<IBasketRepository, CachedBasketRepository>();

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<BasketDbContext>(
            (sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(configuration.GetConnectionString("Database"));
            }
        );

        services.AddHostedService<OutboxProcessor>();
        #endregion

        return services;
    }

    public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
    {
        #region Infrastructure application services
        app.UseMigrations<BasketDbContext>();
        #endregion

        return app;
    }
}
