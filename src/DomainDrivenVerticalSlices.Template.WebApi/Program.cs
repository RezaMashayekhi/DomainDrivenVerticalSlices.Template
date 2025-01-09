using DomainDrivenVerticalSlices.Template.WebApi.Configurations;
using DomainDrivenVerticalSlices.Template.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;
var config = ConfigurationsManager.BuildConfiguration(environment);

var app = builder
    .ConfigureLogger(environment)
    .ConfigureServices(config)
    .Build();

await app.ConfigureApp(config)
    .RunAsync();

/// <summary>
/// It is used in integration testing.
/// </summary>
public partial class Program
{
    protected Program()
    {
    }
}
