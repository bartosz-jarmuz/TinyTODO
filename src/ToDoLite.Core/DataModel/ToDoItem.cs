namespace ToDoLite.Core.DataModel;

public class ToDoItem
{
#pragma warning disable CS8618
    public ToDoItem()
#pragma warning restore CS8618
    {

    }

    public ToDoItem(ClipboardData data, ItemCreationContext context)
    {
        this.PlainText = data.PlainText;
        this.RawData = data.RawData;
        this.CapturedDataType = data.DataType;
        this.ActiveWindowTitle = context.ActiveWindowTitle;
        this.CreatedDateTime = context.TimeStamp;
    }

    public Guid Id { get; set; }
    public bool IsCompleted { get; set; }
    public ClipboardDataType CapturedDataType { get; set; }
    public string? ActiveWindowTitle { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime CompletedDateTime { get; set; }
    public string? PlainText { get; set; }
    public byte[] RawData { get; set; }
}