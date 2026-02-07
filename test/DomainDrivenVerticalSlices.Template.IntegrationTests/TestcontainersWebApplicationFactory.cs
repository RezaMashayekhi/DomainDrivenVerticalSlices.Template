namespace DomainDrivenVerticalSlices.Template.IntegrationTests;

using DomainDrivenVerticalSlices.Template.Infrastructure.Data;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Web application factory that uses Testcontainers for more realistic integration tests.
/// This factory creates a real database container for testing, providing a more production-like environment.
/// Use this when you need to test database-specific behavior that in-memory databases cannot replicate.
/// </summary>
/// <remarks>
/// Note: Testcontainers requires Docker to be running on the host machine.
/// For CI/CD pipelines, ensure Docker is available or use the standard CustomWebApplicationFactory instead.
/// </remarks>
public class TestcontainersWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private IContainer? _dbContainer;
    private string _connectionString = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether to use a containerized database.
    /// When false, falls back to SQLite file-based database for environments without Docker.
    /// </summary>
    public bool UseContainerizedDatabase { get; set; } = true;

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        if (UseContainerizedDatabase && IsDockerAvailable())
        {
            // Create a lightweight Alpine container with SQLite
            // For SQL Server or PostgreSQL, you would use their respective container images
            _dbContainer = new ContainerBuilder()
                .WithImage("alpine:latest")
                .WithCommand("sleep", "infinity")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("echo", "ready"))
                .Build();

            await _dbContainer.StartAsync();
        }

        // For SQLite, we just use a unique temp file
        var dbPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.db");
        _connectionString = $"Data Source={dbPath}";
    }

    /// <inheritdoc/>
    public new async Task DisposeAsync()
    {
        if (_dbContainer is not null)
        {
            await _dbContainer.StopAsync();
            await _dbContainer.DisposeAsync();
        }

        // Clean up SQLite file if exists
        if (_connectionString.Contains("Data Source="))
        {
            var path = _connectionString.Replace("Data Source=", string.Empty);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        await base.DisposeAsync();
    }

    /// <inheritdoc/>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTest");

        builder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            var integrationTestSettings = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.integrationtest.json");
            if (File.Exists(integrationTestSettings))
            {
                config.AddJsonFile(integrationTestSettings);
            }

            // Override connection string with our testcontainer connection
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _connectionString,
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remove the actual DbContext registration
            var descriptorsToRemove = services.Where(d =>
                d.ServiceType == typeof(IDbContextOptionsConfiguration<AppDbContext>)).ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            // Register the DbContext using SQLite with our testcontainer connection
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(_connectionString);
            });

            // Ensure database is created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();
        });
    }

    private static bool IsDockerAvailable()
    {
        try
        {
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = "info",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
            };
            process.Start();
            process.WaitForExit(5000);
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
}
