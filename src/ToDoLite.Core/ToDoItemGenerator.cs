using ToDoLite.Core.ClipboardModel;
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
        if (data == null)
        {
            return null;
        }
        var context = _contextProvider.GetToDoContext();

        if (data is TextualClipboardData textualClipboard)
        {
            return new ToDoItem(textualClipboard, context);
        }
        else if (data is ImageClipboardData imageClipboard)
        {
            return new ToDoItem(imageClipboard, context);
        }

        throw new ArgumentException($"Unexpected Clipboard data type: {data.GetType().Name}");
    }
}