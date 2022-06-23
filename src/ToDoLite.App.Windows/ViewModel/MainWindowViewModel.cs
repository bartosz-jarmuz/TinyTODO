using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ToDoLite.App.Windows.Commands;
using ToDoLite.Core;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;

namespace ToDoLite.App.Windows.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
#pragma warning disable CS8618
        public MainWindowViewModel(){}
#pragma warning restore CS8618

        public MainWindowViewModel(IToDoItemStorage toDoItemStorage)
        {
            Storage = toDoItemStorage;
            ToDoItems = new ObservableCollection<ToDoItemViewModel>();
            ToDoItems.CollectionChanged += OnToDoItemsCollectionChange;
            DeleteItemCommand = new AsyncRelayCommand<ToDoItemViewModel>(DeleteItem);

        }

        private IToDoItemStorage Storage { get; }

        public bool ShowCompleted
        {
            get => Settings.Instance.ShowCompleted; 
            set
            {
                Settings.Instance.ShowCompleted = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ToDoItemViewModel> ToDoItems { get; set; }

        public async Task Initialize()
        {
            var items = await Storage.LoadAllAsync();
            foreach (var item in items)
            {
                ToDoItems.Insert(0, new ToDoItemViewModel(item));
            }
        }

        public void Add(ToDoItem todoItem)
        {
            ToDoItems.Insert(0, new ToDoItemViewModel(todoItem));
        }

        private async Task DeleteItem(ToDoItemViewModel? todoItem)
        {
            if (todoItem == null)
            {
                return;
            }
            ToDoItems.Remove(todoItem);
            await Storage.RemoveAsync(todoItem.Item);
        }

        public ICommand DeleteItemCommand { get; set; }

        void OnToDoItemsCollectionChange(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (ToDoItemViewModel item in e.NewItems)
                    item.PropertyChanged += async (s, e) => await OnToDoItemPropertyChanged(s, e);

            if (e.OldItems != null)
                foreach (ToDoItemViewModel item in e.OldItems)
                    item.PropertyChanged -= async (s, e) => await OnToDoItemPropertyChanged(s, e);
        }

        async Task OnToDoItemPropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            await Storage.SaveAsync();
        }

        public void UpdateSettingBasedProperties()
        {
            OnPropertyChanged(nameof(ShowCompleted));
        }

#region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
#endregion
    }
}
