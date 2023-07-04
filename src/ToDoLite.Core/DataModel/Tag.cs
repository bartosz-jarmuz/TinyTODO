namespace ToDoLite.Core.DataModel;

public class Tag
{
    // ReSharper disable once UnusedMember.Global - EntityFramework PK
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string? Description { get; set; }

    public List<ToDoItem> ToDoItems { get; set; } = new();
}