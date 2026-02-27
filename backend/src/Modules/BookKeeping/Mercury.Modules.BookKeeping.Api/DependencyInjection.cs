using Mercury.Modules.BookKeeping.Application;
using Mercury.Modules.BookKeeping.Infrastructure;
using Mercury.Shared.Application.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mercury.Modules.BookKeeping.Endpoints;

public static class DependencyInjection
{
    public static IServiceCollection AddBookKeepingModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddBookKeepingInfrastructure(configuration)
            .AddBookKeepingApplication();
        
        return services;
    }
}