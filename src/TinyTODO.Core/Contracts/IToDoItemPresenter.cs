using TinyTODO.Core.DataModel;

namespace TinyTODO.Core.Contracts
{
    public interface IToDoItemPresenter
    {
        public void Present(ToDoItem item);
    }
}
