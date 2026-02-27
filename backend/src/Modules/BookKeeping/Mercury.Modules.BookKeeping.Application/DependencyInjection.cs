using Mercury.Shared.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Mercury.Modules.BookKeeping.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddBookKeepingApplication(this IServiceCollection services)
    {
        services.AddBookKeepingFeatures();
        
        return services;
    }

    private static IServiceCollection AddBookKeepingFeatures(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<>)), publicOnly: false)
                .AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces().WithScopedLifetime());
        
        return services;
    }
}