using TinyTODO.Core.DataModel;

namespace TinyTODO.Core.Contracts
{
    public interface IToDoItemStorage : IDisposable
    {
        Task<IEnumerable<ToDoItem>> LoadAllAsync();
        Task InsertAsync(ToDoItem item);
        Task SaveAsync();
    }
}