using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Contracts
{
    public interface IToDoItemPresenter
    {
        public void Present(ToDoItem item);
    }
}
