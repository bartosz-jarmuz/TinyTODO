using Microsoft.EntityFrameworkCore;
using TinyTODO.Core.Contracts;
using TinyTODO.Core.DataModel;

namespace TinyTODO.Core
{
    public class ToDoItemStorage : IToDoItemStorage
    {
        private readonly ToDoDbContext _toDoDbContext;
        private bool disposedValue;

        public ToDoItemStorage()
        {
            _toDoDbContext = new ToDoDbContext();
            _toDoDbContext.Database.EnsureCreated();
        }

        public async Task PersistAsync(ToDoItem item)
        {
            await _toDoDbContext.ToDoItems.AddAsync(item);
            await _toDoDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ToDoItem>> LoadAll()
        {
            return await _toDoDbContext.ToDoItems.ToListAsync();
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
