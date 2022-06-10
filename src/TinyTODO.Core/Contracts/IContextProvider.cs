using TinyTODO.Core.DataModel;

namespace TinyTODO.Core.Contracts;

public interface IContextProvider
{
    ItemCreationContext GetToDoContext();
}
