using TinyTODO.Core.DataModel;

namespace TinyTODO.Core.Contracts
{
    public interface IToDoItemStorage
    {
        void PersistAsync(ToDoItem item);
    }
}