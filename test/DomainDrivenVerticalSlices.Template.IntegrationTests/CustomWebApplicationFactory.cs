namespace DomainDrivenVerticalSlices.Template.IntegrationTests;

using DomainDrivenVerticalSlices.Template.Infrastructure.Data;
using DomainDrivenVerticalSlices.Template.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public List<string> LogMessages { get; } = [];

    public IConfiguration Configuration { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTest");

        builder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            var integrationTestSettings = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.integrationtest.json");
            config.AddJsonFile(integrationTestSettings);
            Configuration = config.Build();
        });

        builder.ConfigureServices(services =>
        {
            // Remove the actual DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Register the DbContext using in-memory database
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });

            // Add HTTPS redirection and HSTS services
            services.AddHttpsRedirection(options =>
            {
                var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
                options.HttpsPort = configuration!.GetValue<int>("HttpsPort");
            });
            services.AddHsts(options =>
            {
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });
        });

        builder.ConfigureLogging((hostingContext, logging) =>
        {
            logging.ClearProviders();
            logging.AddProvider(new ListLoggerProvider(LogMessages));
        });
    }
}
