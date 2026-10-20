namespace DomainDrivenVerticalSlices.Template.Api.Common.Persistence;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

internal static class SqliteErrors
{
    // SQLITE_CONSTRAINT_UNIQUE, not every constraint or every database failure.
    public static bool IsUniqueConstraintViolation(DbUpdateException exception) =>
        exception.InnerException is SqliteException { SqliteExtendedErrorCode: 2067 };
}
