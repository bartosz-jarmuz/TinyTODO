using Microsoft.EntityFrameworkCore;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;

namespace ToDoLite.Core
{
    public class ToDoItemStorage : IToDoItemStorage
    {
        private readonly ToDoLiteDbContext _toDoDbContext;
        private bool disposedValue;

        public ToDoItemStorage()
        {
            _toDoDbContext = new ToDoLiteDbContext();
        }

        public async Task InsertAsync(ToDoItem item)
        {
            await _toDoDbContext.ToDoItems.AddAsync(item);
            await _toDoDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ToDoItem>> LoadAllAsync()
        {
            return await _toDoDbContext.ToDoItems.ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _toDoDbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _toDoDbContext?.Dispose();
                }
                disposedValue = true;
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
