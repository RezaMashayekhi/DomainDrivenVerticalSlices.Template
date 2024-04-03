namespace DomainDrivenVerticalSlices.Template.WebApi.Extensions;

using DomainDrivenVerticalSlices.Template.Application.Extensions;
using DomainDrivenVerticalSlices.Template.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder, IConfiguration config)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(config);
        builder.Services.AddLogging();
        return builder;
    }
}
