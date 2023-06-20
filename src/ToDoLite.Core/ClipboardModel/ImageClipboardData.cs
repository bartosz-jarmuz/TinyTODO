using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.ClipboardModel;

public class ImageClipboardData : ClipboardData
{
    public ImageClipboardData(byte[] rawData) : base(CapturedDataType.Image, string.Empty, rawData)
    {
    }
}