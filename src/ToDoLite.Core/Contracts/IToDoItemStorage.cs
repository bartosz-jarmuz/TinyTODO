using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Contracts
{
    public interface IToDoItemStorage : IDisposable
    {
        Task<IEnumerable<ToDoItem>> LoadAllAsync();
        Task InsertAsync(ToDoItem item);
        Task RemoveAsync(ToDoItem item);
        Task SaveAsync();
        Task RecreateDatabaseAsync();
    }
}