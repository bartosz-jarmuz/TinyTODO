using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Contracts
{
    public interface ITagRepository : IDisposable
    {
        Task<IEnumerable<Tag>> LoadAllUsedTagsAsync();
        Task<Tag> GetOrCreateTagAsync(string name, string? description);

        public event EventHandler<TagAssignedEventArgs> TagAssigned;
    }
}