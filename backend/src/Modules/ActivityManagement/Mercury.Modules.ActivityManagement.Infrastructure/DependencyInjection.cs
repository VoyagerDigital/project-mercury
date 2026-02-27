using Mercury.Modules.ActivityManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mercury.Modules.ActivityManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddActivityManagementInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddActivityManagementPersistence(configuration);

        return services;
    }

    private static IServiceCollection AddActivityManagementPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ActivityManagementDbContext>(options =>
        {
            // TODO: Add Options pattern

            options.UseNpgsql(configuration.GetSection("Databases").GetSection("ActivityManagement").Value);
        });

        return services;
    }
}