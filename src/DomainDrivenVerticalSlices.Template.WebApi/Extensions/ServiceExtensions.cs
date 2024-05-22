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
#if INCLUDE_REACT
        var corsPolicyName = config.GetSection("CorsSettings:PolicyName").Value ?? throw new InvalidOperationException("CORS policy name not found in configuration.");
        var allowedOrigins = config.GetSection("CorsSettings:AllowedOrigins").Get<string[]>() ?? throw new InvalidOperationException("CORS AllowedOrigins not found in configuration.");
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(corsPolicyName, builder =>
            {
                builder.WithOrigins(allowedOrigins)
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
#endif
        return builder;
    }
}
