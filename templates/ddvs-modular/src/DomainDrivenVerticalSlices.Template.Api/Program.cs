using System.Reflection;
using DomainDrivenVerticalSlices.Template.Api.Common.Endpoints;
using DomainDrivenVerticalSlices.Template.Api.Common.Persistence;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1;
#if USE_ASPIRE
using DomainDrivenVerticalSlices.Template.ServiceDefaults;
#endif

var builder = WebApplication.CreateBuilder(args);

#if USE_ASPIRE
builder.AddServiceDefaults();
#endif

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "DomainDrivenVerticalSlices.Template API",
        Version = "v1",
    });
});

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddEntity1Module(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "DomainDrivenVerticalSlices.Template API (v1)");
    });

    await app.InitializeDatabaseAsync();
}

app.UseHttpsRedirection();

#if USE_ASPIRE
app.MapDefaultEndpoints();
#endif

app.MapEndpoints();

await app.RunAsync();

public partial class Program
{
    protected Program()
    {
    }
}
