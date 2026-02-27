using FastEndpoints;
using FastEndpoints.Swagger;
using Mercury.Modules.ActivityManagement.Endpoints;
using Mercury.Modules.BookKeeping.Endpoints;
using ActivityManagementEndpointsDependencyInjection = Mercury.Modules.ActivityManagement.Endpoints.DependencyInjection;
using BookKeepingEndpointsDependencyInjection = Mercury.Modules.BookKeeping.Endpoints.DependencyInjection;

namespace Mercury.App.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterApplicationModules(configuration)
            .RegisterFastEndpoints();
        
        return services;
    }

    private static IServiceCollection RegisterApplicationModules(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddBookKeepingModule(configuration)
            .AddActivityManagementModule(configuration);
        
        return services;
    }

    private static IServiceCollection RegisterFastEndpoints(this IServiceCollection services)
    {
        services.AddFastEndpoints(config =>
            {
                config.Assemblies =
                [
                    typeof(BookKeepingEndpointsDependencyInjection).Assembly,
                    typeof(ActivityManagementEndpointsDependencyInjection).Assembly
                ];
            })
            .SwaggerDocument(o =>
            {
                o.MaxEndpointVersion = 1;
                o.DocumentSettings = s =>
                {
                    s.DocumentName = "Version 1";
                    s.Title = "Mercury API";
                    s.Version = "v1";
                };
            });
        
        return services;
    }
}