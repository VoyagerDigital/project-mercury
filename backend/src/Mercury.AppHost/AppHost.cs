using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgresServer = builder.AddPostgres("DatabaseServer")
    .WithPgAdmin();

IResourceBuilder<PostgresDatabaseResource> database = postgresServer
    .AddDatabase("MercuryDb");

IResourceBuilder<ProjectResource> dataSeeder = builder.AddProject<Mercury_DataSeeder>("DataSeeder")
    .WaitFor(database)
    .WithEnvironment("Databases__BookKeeping", database)
    .WithEnvironment("Databases__ActivityManagement", database);

IResourceBuilder<ProjectResource> api = builder.AddProject<Mercury_App>("Api")
    .WaitFor(database)
    .WaitFor(dataSeeder)
    .WithEnvironment("Databases__BookKeeping", database)
    .WithEnvironment("Databases__ActivityManagement", database);

builder.AddViteApp("Frontend", "../../../frontend")
    .WithReference(api)
    .WithExternalHttpEndpoints();

builder.Build()
    .Run();