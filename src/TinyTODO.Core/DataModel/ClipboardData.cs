namespace TinyTODO.Core.DataModel;

public record ClipboardData
{
    public ClipboardData(ClipboardDataType dataType, string? plainText, byte[] rawData)
    {
        DataType = dataType;
        PlainText = plainText;
        RawData = rawData;
    }

    public ClipboardDataType DataType { get; }
    public string? PlainText { get; }
    public byte[] RawData { get; }
}
