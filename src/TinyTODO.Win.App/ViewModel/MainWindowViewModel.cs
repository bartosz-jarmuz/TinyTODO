    using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyTODO.Core.DataModel;

namespace TinyTODO.App.Windows.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ToDoItem> Items { get; set; } = new ObservableCollection<ToDoItem>();

        public void Add(ToDoItem todoItem)
        {
            Items.Insert(0,todoItem);
        }
    }
}
