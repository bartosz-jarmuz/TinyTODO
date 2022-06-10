using System;

namespace TinyTODO.Core.DataModel
{
    public record ItemCreationContext
    {
        public ItemCreationContext(string? activeWindowTitle)
        {
            TimeStamp = DateTime.UtcNow;
            ActiveWindowTitle = activeWindowTitle;
        }

        public string? ActiveWindowTitle { get; init; }
        public DateTime TimeStamp { get; init; }
    }
}
