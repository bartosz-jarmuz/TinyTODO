using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.ClipboardModel;

public class TextualClipboardData : ClipboardData
{
    public TextualClipboardData(CapturedDataType dataType, string? plainText, byte[] rawData) : base(dataType, plainText, rawData)
    {
    }
}