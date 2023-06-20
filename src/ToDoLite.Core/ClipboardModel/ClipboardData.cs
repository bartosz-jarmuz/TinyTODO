using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.ClipboardModel;

public abstract class ClipboardData
{
    protected ClipboardData(CapturedDataType dataType, string? plainText, byte[] rawData)
    {
        DataType = dataType;
        PlainText = plainText;
        RawData = rawData;
    }

    public CapturedDataType DataType { get; }
    public string? PlainText { get; }
    public byte[] RawData { get; }
}