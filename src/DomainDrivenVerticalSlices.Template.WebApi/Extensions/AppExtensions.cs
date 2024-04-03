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

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}
