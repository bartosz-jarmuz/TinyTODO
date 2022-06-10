    using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyTODO.App.Windows.Model;
using TinyTODO.Core.DataModel;

namespace TinyTODO.App.Windows.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<DisplayableToDoItem> Items { get; set; } = new ObservableCollection<DisplayableToDoItem>();

        public void Add(ToDoItem todoItem)
        {
            Items.Insert(0, new DisplayableToDoItem(todoItem));
        }
    }
}
