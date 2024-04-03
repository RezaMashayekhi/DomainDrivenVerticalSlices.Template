namespace DomainDrivenVerticalSlices.Template.Infrastructure;

using DomainDrivenVerticalSlices.Template.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public static class DatabaseInitializer
{
    public static void Initialize(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='__EFMigrationsHistory'";
        var result = command.ExecuteScalar();

        if (result == null)
        {
            using var context = new AppDbContext(
                new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options);

            context.Database.Migrate();
        }
    }
}
