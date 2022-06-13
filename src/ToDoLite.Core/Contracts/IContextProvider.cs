using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Contracts;

public interface IContextProvider
{
    ItemCreationContext GetToDoContext();
}
