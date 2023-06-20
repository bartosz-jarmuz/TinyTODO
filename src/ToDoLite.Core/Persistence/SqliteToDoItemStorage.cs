using Microsoft.EntityFrameworkCore;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Persistence
{
    public sealed class SqliteToDoItemStorage : IToDoItemStorage
    {
        private readonly ToDoLiteDbContext _toDoDbContext;
        private bool _isDisposed;

        public SqliteToDoItemStorage()
        {
            _toDoDbContext = new ToDoLiteDbContext();
        }

        public async Task InsertAsync(ToDoItem item)
        {
            await _toDoDbContext.ToDoItems.AddAsync(item);
            await _toDoDbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(ToDoItem item)
        {
            _toDoDbContext.ToDoItems.Remove(item);
            await _toDoDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ToDoItem>> LoadAllAsync()
        {
            return await _toDoDbContext.ToDoItems.Include(x=>x.Images).ToListAsync();
        }

        public async Task RecreateDatabaseAsync()
        {
            await _toDoDbContext.RecreateDatabaseAsync();
        }

        public async Task SaveAsync()
        {
            await _toDoDbContext.SaveChangesAsync();
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _toDoDbContext?.Dispose();
                }
                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
