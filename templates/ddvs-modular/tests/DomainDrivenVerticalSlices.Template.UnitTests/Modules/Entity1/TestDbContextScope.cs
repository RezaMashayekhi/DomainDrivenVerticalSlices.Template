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

        await DeleteDatabaseFileAsync(_databasePath);
    }

    private static async Task DeleteDatabaseFileAsync(string databasePath)
    {
        const int maxAttempts = 5;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            if (!File.Exists(databasePath))
            {
                return;
            }

            try
            {
                File.Delete(databasePath);
                return;
            }
            catch (IOException) when (attempt < maxAttempts)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(50 * attempt));
            }
            catch (UnauthorizedAccessException) when (attempt < maxAttempts)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(50 * attempt));
            }
            catch (IOException)
            {
                return;
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }
        }
    }
}
