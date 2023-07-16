using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ToDoLite.Core.Persistence;

namespace ToDoLite.App.Windows.UnitTests.Helpers
{

    public static class TestContextProvider
    {
        public static TestContext Get()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            var _contextOptions = new DbContextOptionsBuilder<ToDoLiteDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            using var context = new ToDoLiteDbContext(_contextOptions);

            context.Database.EnsureCreated();

            return new (_connection, context);

        }
    }


}