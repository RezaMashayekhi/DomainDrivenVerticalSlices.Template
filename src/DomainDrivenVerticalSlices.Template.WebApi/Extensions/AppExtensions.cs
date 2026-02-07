namespace DomainDrivenVerticalSlices.Template.WebApi.Extensions;

using DomainDrivenVerticalSlices.Template.Infrastructure;
using DomainDrivenVerticalSlices.Template.ServiceDefaults;
using DomainDrivenVerticalSlices.Template.WebApi.Infrastructure;

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
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Controllers (v1)");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "Minimal APIs (v2)");
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

        // Map traditional controllers
        app.MapControllers();

        // Map Minimal API endpoints (alternative to controllers)
        app.MapEndpoints();

        return app;
    }
}
