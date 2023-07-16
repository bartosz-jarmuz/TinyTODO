using Microsoft.Data.Sqlite;
using System;
using ToDoLite.Core.Persistence;

namespace ToDoLite.App.Windows.UnitTests.Helpers
{
    public class TestContext : IDisposable
    {
        public TestContext(SqliteConnection connection, ToDoLiteDbContext dbContext)
        {
            _connection = connection;
            DbContext = dbContext;
        }
        private readonly SqliteConnection _connection;

        public ToDoLiteDbContext DbContext { get; }
        public void Dispose()
        {
            _connection.Dispose();
            DbContext.Dispose();
        }
    }
}