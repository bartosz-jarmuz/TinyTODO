using System;

namespace TinyTODO.Core.DataModel
{
    public record ToDoContext
    {
        public ToDoContext(string? activeWindowTitle)
        {
            TimeStamp = DateTime.UtcNow;
            ActiveWindowTitle = activeWindowTitle;
        }

        public string? ActiveWindowTitle { get; init; }
        public DateTime TimeStamp { get; init; }
    }
}
