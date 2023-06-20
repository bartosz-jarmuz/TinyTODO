using ToDoLite.Core.ClipboardModel;

namespace ToDoLite.Core.Contracts;

public interface IClipboardDataProvider
{
    ClipboardData? GetData();
}
