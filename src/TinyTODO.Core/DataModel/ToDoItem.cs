namespace TinyTODO.Core.DataModel
{
    public class ToDoItem
    {
        public ToDoItem()
        {

        }

        public ToDoItem(ClipboardData Data, ItemCreationContext Context)
        {
            this.PlainTextData = Data.PlainText;
            this.RawData = Data.RawData;
            this.DataType = Data.DataType;
            this.ActiveWindowTitle = Context.ActiveWindowTitle;
            this.CreatedDateTime = Context.TimeStamp;
        }

        public Guid Id { get; set; }
        public bool IsCompleted { get; set; }
        public ClipboardDataType DataType { get; set; }
        public string? ActiveWindowTitle { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime CompletedDateTime { get; set; }
        public string? PlainTextData { get; set; }
        public byte[] RawData { get; set; }
    }
}
