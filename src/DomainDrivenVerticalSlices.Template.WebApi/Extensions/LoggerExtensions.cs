namespace DomainDrivenVerticalSlices.Template.WebApi.Extensions;

using Serilog;

public static class LoggerExtensions
{
    public static WebApplicationBuilder ConfigureLogger(this WebApplicationBuilder builder, string environment)
    {
        if (environment != "IntegrationTest")
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            builder.Host.UseSerilog();
        }

        return builder;
    }
}
