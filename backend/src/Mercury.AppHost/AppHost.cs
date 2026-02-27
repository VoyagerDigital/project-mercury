using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgresServer = builder.AddPostgres("DatabaseServer");

IResourceBuilder<PostgresDatabaseResource> database = postgresServer
    .AddDatabase("MercuryDb");

IResourceBuilder<ProjectResource> dataSeeder = builder.AddProject<Mercury_DataSeeder>("DataSeeder")
    .WaitFor(database)
    .WithEnvironment("Databases__BookKeeping", database);

IResourceBuilder<ProjectResource> api = builder.AddProject<Mercury_App>("Api")
    .WaitFor(database)
    .WaitFor(dataSeeder)
    .WithEnvironment("Databases__BookKeeping", database);

builder.AddViteApp("Frontend", "../../../frontend")
    .WithReference(api)
    .WithExternalHttpEndpoints();

builder.Build()
    .Run();