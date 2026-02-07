var builder = DistributedApplication.CreateBuilder(args);

// Add the WebApi project
var webApi = builder.AddProject<Projects.DomainDrivenVerticalSlices_Template_WebApi>("webapi")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName);
#if INCLUDE_REACT

// Add the React UI (Vite dev server)
builder.AddViteApp("reactui", "../DomainDrivenVerticalSlices.Template.UI.React")
    .WithReference(webApi);
#endif

builder.Build().Run();
