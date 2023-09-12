using ToDoLite.Core.DataModel;

namespace ToDoLite.App.Windows.ViewModel
{
    public class TagFilterViewModel : TagViewModel
    {
        private int usageCount;

        public TagFilterViewModel(Tag tag, int usageCount) : base(tag)
        {
            UsageCount = usageCount;
        }

        public int UsageCount
        {
            get => usageCount;
            set
            {
                usageCount = value;
                OnPropertyChanged();
            }
        }

    }
}
