using Mercury.App.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .RegisterApplicationServices(builder.Configuration);

WebApplication app = builder.Build();

app.ConfigureApplication();

app.Run();