namespace DomainDrivenVerticalSlices.Template.WebApi.Extensions;

using DomainDrivenVerticalSlices.Template.Application.Extensions;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Infrastructure.Extensions;
using DomainDrivenVerticalSlices.Template.ServiceDefaults;
using DomainDrivenVerticalSlices.Template.WebApi.Services;

public static class ServiceExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder, IConfiguration config)
    {
        // Add Aspire service defaults (OpenTelemetry, health checks, resilience)
        builder.AddServiceDefaults();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "Entity1 API - Controllers",
                Version = "v1",
                Description = "Traditional MVC Controllers at /api/Entity1",
            });

            options.SwaggerDoc("v2", new()
            {
                Title = "Entity1 API - Minimal APIs",
                Version = "v2",
                Description = "Modern Minimal API endpoints at /api/v2/Entity1",
            });

            // Assign endpoints to the correct Swagger doc based on group name
            options.DocInclusionPredicate((docName, api) =>
            {
                if (docName == "v2")
                {
                    return api.GroupName == "v2";
                }

                // "v1" doc gets everything without a group name (i.e., controllers)
                return api.GroupName is null;
            });
        });

        // Register HttpContextAccessor and CurrentUser for audit trail
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IUser, CurrentUser>();

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
