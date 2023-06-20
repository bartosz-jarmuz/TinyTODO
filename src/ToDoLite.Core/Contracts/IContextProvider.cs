using ToDoLite.Core.ClipboardModel;

namespace ToDoLite.Core.Contracts;

public interface IContextProvider
{
    ItemCreationContext GetToDoContext();
}
