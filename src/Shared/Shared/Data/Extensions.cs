using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Seed;

namespace Shared.Data;

public static class Extensions
{
    public static IApplicationBuilder UseMigrations<TDbContext>(
        this IApplicationBuilder applicationBuilder
    )
        where TDbContext : DbContext
    {
        MigrateAsync<TDbContext>(applicationBuilder.ApplicationServices).GetAwaiter().GetResult();
        SeedDataAsync<TDbContext>(applicationBuilder.ApplicationServices).GetAwaiter().GetResult();

        return applicationBuilder;
    }

    private static async Task MigrateAsync<TDbContext>(IServiceProvider serviceProvider)
        where TDbContext : DbContext
    {
        using var serviceScope = serviceProvider.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();
        await context.Database.MigrateAsync();
    }

    private static async Task SeedDataAsync<TDbContext>(IServiceProvider serviceProvider)
        where TDbContext : DbContext
    {
        using var serviceScope = serviceProvider.CreateScope();
        var seeders = serviceScope.ServiceProvider.GetServices<IDataSeeder>();

        foreach (var seeder in seeders)
        {
            await seeder.SeedAllAsync();
        }
    }
}
