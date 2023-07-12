using Microsoft.EntityFrameworkCore;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Persistence
{
    public sealed class SqliteToDoItemStorage : IToDoItemStorage, ITagRepository
    {
        private readonly ToDoLiteDbContext _toDoDbContext;
        private bool _isDisposed;

        public event EventHandler<TagAssignedEventArgs>? TagAssigned;

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
            return await _toDoDbContext.ToDoItems.Include(x=>x.Images).Include(x=>x.Tags).ToListAsync();
        }

        public async Task<IEnumerable<Tag>> LoadAllUsedTagsAsync()
        {
            return await _toDoDbContext.Tags.Where(x=>x.ToDoItems.Count > 0).ToListAsync();

        }

        public async Task<Tag> GetOrCreateTagAsync(string name, string? description)
        {
            var tag = await _toDoDbContext.Tags.FirstOrDefaultAsync(x => x.Name == name);
            if (tag == null)
            {
                tag = new Tag(name)
                {
                    Description = description
                };
                _toDoDbContext.Tags.Add(tag);
            }
            OnTagAssigned(tag);
            return tag;
        }

        private void OnTagAssigned(Tag newTag)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/events/how-to-publish-events-that-conform-to-net-framework-guidelines
            var copyOfEvent = TagAssigned;

            if (copyOfEvent != null)
            {
                copyOfEvent(this, new TagAssignedEventArgs(newTag));
            }
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
