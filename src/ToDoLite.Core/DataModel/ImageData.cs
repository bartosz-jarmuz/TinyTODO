namespace ToDoLite.Core.DataModel;

#pragma warning disable CS8618 //EF Instantiated class
public class ImageData
{
    // ReSharper disable once UnusedMember.Global - EntityFramework PK
    public Guid Id { get; set; }
    public ToDoItem ToDoItem { get; set; }
    public byte[] Bytes { get; set; }
}