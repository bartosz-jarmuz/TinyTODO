using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Contracts
{
    public class TagAssignedEventArgs : EventArgs
    {
        public TagAssignedEventArgs(Tag newTag)
        {
            Tag = newTag;
        }

        public Tag Tag { get; }
    }
}