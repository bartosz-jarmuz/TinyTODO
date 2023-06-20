namespace ToDoLite.Core.ClipboardModel
{
    public record ItemCreationContext(string? ActiveWindowTitle)
    {
        public DateTime TimeStamp { get; init; } = DateTime.UtcNow;
    }
}
