using Mercury.Modules.BookKeeping.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mercury.Modules.BookKeeping.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddBookKeepingInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddBookKeepingPersistence(configuration);
        
        return services;
    }

    private static IServiceCollection AddBookKeepingPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<BookKeepingDbContext>(options =>
        {
            // TODO: Add Options pattern
            
            options.UseNpgsql(configuration.GetSection("Databases").GetSection("BookKeeping").Value);
        });
        
        return services;
    }
}