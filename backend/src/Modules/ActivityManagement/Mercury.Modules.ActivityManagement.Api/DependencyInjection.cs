using Mercury.Modules.ActivityManagement.Application;
using Mercury.Modules.ActivityManagement.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mercury.Modules.ActivityManagement.Endpoints;

public static class DependencyInjection
{
    public static IServiceCollection AddActivityManagementModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddActivityManagementInfrastructure(configuration)
            .AddActivityManagementApplication();

        return services;
    }
}
