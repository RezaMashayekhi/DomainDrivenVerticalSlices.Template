var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DomainDrivenVerticalSlices_Template_Api>("api")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName);

builder.Build().Run();
