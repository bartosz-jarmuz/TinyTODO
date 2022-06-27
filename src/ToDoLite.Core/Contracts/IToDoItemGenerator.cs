using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Contracts
{
    public interface IToDoItemGenerator
    {
        ToDoItem? GenerateItem();
    }
}
