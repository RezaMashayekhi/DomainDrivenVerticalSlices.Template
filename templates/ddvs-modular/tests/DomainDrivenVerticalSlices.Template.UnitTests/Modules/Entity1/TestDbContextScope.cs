namespace DomainDrivenVerticalSlices.Template.UnitTests.Modules.Entity1;

using DomainDrivenVerticalSlices.Template.Api.Common.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

internal sealed class TestDbContextScope : IAsyncDisposable
{
    private readonly string _databasePath;

    private TestDbContextScope(AppDbContext dbContext, string databasePath)
    {
        DbContext = dbContext;
        _databasePath = databasePath;
    }

    public AppDbContext DbContext { get; }

    public static async Task<TestDbContextScope> CreateAsync()
    {
        string databasePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.db");

        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={databasePath}")
            .Options;

        AppDbContext dbContext = new(options);

        await dbContext.Database.EnsureCreatedAsync();

        return new TestDbContextScope(dbContext, databasePath);
    }

    public async ValueTask DisposeAsync()
    {
        await DbContext.DisposeAsync();
        SqliteConnection.ClearAllPools();

        if (File.Exists(_databasePath))
        {
            File.Delete(_databasePath);
        }
    }
}
