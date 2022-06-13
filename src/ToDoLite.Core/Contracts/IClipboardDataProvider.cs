using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Contracts;

public interface IClipboardDataProvider
{
    ClipboardData? GetData();
}
