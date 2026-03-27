namespace DomainDrivenVerticalSlices.Template.WebApi.Extensions;

using DomainDrivenVerticalSlices.Template.Infrastructure;
using DomainDrivenVerticalSlices.Template.ServiceDefaults;
#if INCLUDE_MINIMAL_API
using DomainDrivenVerticalSlices.Template.WebApi.Infrastructure;
#endif

public static class AppExtensions
{
    public static WebApplication ConfigureApp(this WebApplication app, IConfiguration config)
    {
        // Map Aspire health check endpoints (/health, /alive)
        app.MapDefaultEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Entity1 API (v1)");
            });

            var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("No connection string named 'DefaultConnection' found in the configuration.");
            DatabaseInitializer.Initialize(connectionString);
        }

#if INCLUDE_REACT
        var corsPolicyName = config.GetSection("CorsSettings:PolicyName").Value ?? throw new InvalidOperationException("CORS policy name not found in configuration.");
        app.UseCors(corsPolicyName);
#endif
        app.UseHttpsRedirection();
        app.UseAuthorization();

#if INCLUDE_CONTROLLERS
        app.MapControllers();
#endif
#if INCLUDE_MINIMAL_API
        app.MapEndpoints();
#endif

        return app;
    }
}
