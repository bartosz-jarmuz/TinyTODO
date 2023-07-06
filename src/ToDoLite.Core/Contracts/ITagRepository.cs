using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Contracts
{
    public interface ITagRepository : IDisposable
    {
        Task<IEnumerable<Tag>> LoadAllTagsAsync();
        Task<Tag> GetOrCreateTagAsync(string name, string? description);
    }
}