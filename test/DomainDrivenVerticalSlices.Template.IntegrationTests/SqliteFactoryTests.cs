namespace DomainDrivenVerticalSlices.Template.IntegrationTests;

public class SqliteFactoryTests(SqliteWebApplicationFactory factory) : IClassFixture<SqliteWebApplicationFactory>
{
    [Fact]
    public async Task GetAll_UsesRealSqliteWithoutDocker()
    {
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/api/Entity1/all");
        response.EnsureSuccessStatusCode();
    }
}
