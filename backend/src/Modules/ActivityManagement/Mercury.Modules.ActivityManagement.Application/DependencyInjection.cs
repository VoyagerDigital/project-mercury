using Mercury.Shared.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Mercury.Modules.ActivityManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddActivityManagementApplication(this IServiceCollection services)
    {
        services.AddActivityManagementFeatures();

        return services;
    }

    private static IServiceCollection AddActivityManagementFeatures(this IServiceCollection services)
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