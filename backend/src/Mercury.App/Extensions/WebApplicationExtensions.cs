using FastEndpoints;
using FastEndpoints.Swagger;
using Mercury.Modules.ActivityManagement.Infrastructure.Database;
using Mercury.Modules.BookKeeping.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Mercury.App.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.ConfigureFastEndpoints()
            .UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
            app.ConfigureDevelopmentMiddleware();
        
        app.MigrateDatabases();
        
        return app;
    }
    
    private static WebApplication ConfigureDevelopmentMiddleware(this WebApplication app)
    {
        app.UseSwaggerGen();
            
        return app;
    }
    
    private static WebApplication ConfigureFastEndpoints(this WebApplication app)
    {
        app.UseFastEndpoints(config =>
        {
            config.Binding.JsonExceptionStatusCode = 400;
            config.Errors.UseProblemDetails();
            config.Endpoints.RoutePrefix = "api";
            config.Versioning.Prefix = "v";
            config.Versioning.PrependToRoute = true;
            config.Endpoints.Configurator = ep =>
            {
                ep.DontThrowIfValidationFails();
            };
        });
        
        return app;
    }

    private static void MigrateDatabases(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        BookKeepingDbContext bookKeepingDbContext = scope.ServiceProvider.GetRequiredService<BookKeepingDbContext>();
        ActivityManagementDbContext activityManagementDbContext = scope.ServiceProvider.GetRequiredService<ActivityManagementDbContext>();
        
        bookKeepingDbContext.Database
            .Migrate();

        activityManagementDbContext.Database
            .Migrate();
    }
}