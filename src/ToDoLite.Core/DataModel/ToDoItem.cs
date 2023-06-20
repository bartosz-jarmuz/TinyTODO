using ToDoLite.Core.ClipboardModel;

namespace ToDoLite.Core.DataModel;

public class ToDoItem
{
    public ToDoItem() { }

    public ToDoItem(TextualClipboardData data, ItemCreationContext context) : this(context)
    {
        PlainText = data.PlainText;
        RawData = data.RawData;
        CapturedDataType = data.DataType;
    }

    public ToDoItem(ImageClipboardData data, ItemCreationContext context) : this(context)
    {
        PlainText = data.PlainText;
        CapturedDataType = data.DataType;
        Images.Add(new()
            {
                ToDoItem = this,
                Bytes = data.RawData
            });
    }

    private ToDoItem(ItemCreationContext context)
    {
        ActiveWindowTitle = context.ActiveWindowTitle;
        CreatedDateTime = context.TimeStamp;
    }

    // ReSharper disable once UnusedMember.Global - EntityFramework PK
    public Guid Id { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public CapturedDataType CapturedDataType { get; set; }
    
    public string? ActiveWindowTitle { get; set; }
    
    public DateTime CreatedDateTime { get; set; }
    
    public DateTime CompletedDateTime { get; set; }
    
    /// <summary>
    /// Plain text data is used for searching through the items
    /// </summary>
    public string? PlainText { get; set; }
    
    public byte[] RawData { get; set; } = Array.Empty<byte>();
    
    public ICollection<ImageData> Images { get; set; } = new List<ImageData>();
}