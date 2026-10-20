namespace DomainDrivenVerticalSlices.Template.IntegrationTests;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class TestHostConfigurationTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TestHost_HasSettingsWithExactEnvironmentCasing(bool sqlite)
    {
        using WebApplicationFactory<Program> factory = sqlite
            ? new SqliteWebApplicationFactory()
            : new CustomWebApplicationFactory();
        string environment = factory.Services.GetRequiredService<IHostEnvironment>().EnvironmentName;
        string settings = $"appsettings.{environment}.json";

        var files = Directory.EnumerateFiles(AppContext.BaseDirectory).Select(Path.GetFileName);
        Assert.Contains(settings, files, StringComparer.Ordinal);
    }
}
