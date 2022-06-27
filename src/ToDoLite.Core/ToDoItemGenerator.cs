using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;

namespace ToDoLite.Core;

public class ToDoItemGenerator : IToDoItemGenerator
{
    private readonly IClipboardDataProvider _clipboardProvider;
    private readonly IContextProvider _contextProvider;

    public ToDoItemGenerator(IClipboardDataProvider clipboardProvider, IContextProvider contextProvider)
    {
        _clipboardProvider = clipboardProvider;
        _contextProvider = contextProvider;
    }

    public ToDoItem? GenerateItem()
    {
        var data = _clipboardProvider.GetData();
        var context = _contextProvider.GetToDoContext();
        if (data == null)
        {
            return null;
        }

        return new ToDoItem(data, context);
    }
}