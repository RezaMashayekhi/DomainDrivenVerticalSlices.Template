namespace DomainDrivenVerticalSlices.Template.WebApi.Extensions;

using DomainDrivenVerticalSlices.Template.Infrastructure;

public static class AppExtensions
{
    public static WebApplication ConfigureApp(this WebApplication app, IConfiguration config)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("No connection string named 'DefaultConnection' found in the configuration.");
            DatabaseInitializer.Initialize(connectionString);
        }

#if INCLUDE_REACT
        var corsPolicyName = config.GetSection("CorsSettings:PolicyName").Value ?? throw new InvalidOperationException("CORS policy name not found in configuration.");
        app.UseCors(corsPolicyName);
#endif
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}
