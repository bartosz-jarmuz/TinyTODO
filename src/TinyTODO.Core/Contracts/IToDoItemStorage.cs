using TinyTODO.Core.DataModel;

namespace TinyTODO.Core.Contracts
{
    public interface IToDoItemStorage : IDisposable
    {
        Task<IEnumerable<ToDoItem>> LoadAll();
        Task PersistAsync(ToDoItem item);
    }
}