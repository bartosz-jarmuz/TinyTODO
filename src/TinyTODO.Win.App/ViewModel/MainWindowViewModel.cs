using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TinyTODO.App.Windows.Model;
using TinyTODO.Core;
using TinyTODO.Core.Contracts;
using TinyTODO.Core.DataModel;

namespace TinyTODO.App.Windows.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool showCompleted = Settings.Instance.ShowCompleted;

        public IToDoItemStorage Storage { get; set; }

        public bool ShowCompleted
        {
            get => showCompleted; 
            set
            {
                showCompleted = value;
                Settings.Instance.ShowCompleted = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            this.Items = new ObservableCollection<DisplayableToDoItem>();
            this.Items.CollectionChanged += MyItemsSource_CollectionChanged;
        }

        public ObservableCollection<DisplayableToDoItem> Items { get; set; }

        public void Add(ToDoItem todoItem)
        {
            Items.Insert(0, new DisplayableToDoItem(todoItem));
        }

        void MyItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (DisplayableToDoItem item in e.NewItems)
                    item.PropertyChanged += async (s, e) => await MyType_PropertyChanged(s, e);

            if (e.OldItems != null)
                foreach (DisplayableToDoItem item in e.OldItems)
                    item.PropertyChanged -= async (s, e) => await MyType_PropertyChanged(s, e);
        }

        async Task MyType_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await Storage.SaveAsync();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
