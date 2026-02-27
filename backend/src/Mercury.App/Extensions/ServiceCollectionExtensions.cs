using FastEndpoints;
using FastEndpoints.Swagger;
using Mercury.Modules.BookKeeping.Endpoints;

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
        services.AddBookKeepingModule(configuration);
        
        return services;
    }

    private static IServiceCollection RegisterFastEndpoints(this IServiceCollection services)
    {
        services.AddFastEndpoints(config =>
            {
                config.Assemblies =
                [
                    typeof(DependencyInjection).Assembly
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