namespace DomainDrivenVerticalSlices.Template.WebApi.Configurations;

public static class ConfigurationsManager
{
    public static IConfiguration BuildConfiguration(string environment)
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .Build();
    }
}
