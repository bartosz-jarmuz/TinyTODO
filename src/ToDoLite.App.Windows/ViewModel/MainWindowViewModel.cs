using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoLite.Core.Contracts;
using Settings = ToDoLite.Core.Persistence.Settings;

namespace ToDoLite.App.Windows.ViewModel
{
    public class MainWindowViewModel : ObservableObject, IDisposable
    {
#pragma warning disable CS8618
        public MainWindowViewModel(){}
#pragma warning restore CS8618

        public MainWindowViewModel(IToDoItemStorage toDoItemStorage, IToDoItemGenerator toDoItemGenerator, IConfirmationEmitter confirmationEmitter)
        {
            _storage = toDoItemStorage;
            _toDoItemGenerator = toDoItemGenerator;
            _confirmationEmitter = confirmationEmitter;
            ToDoItems = new ObservableCollection<ToDoItemViewModel>();
            ToDoItems.CollectionChanged += OnToDoItemsCollectionChange;
            DeleteItemCommand = new AsyncRelayCommand<ToDoItemViewModel>(DeleteItem);
            OpenOptionsWindowCommand = new RelayCommand(ShowOptionsWindow);
        }

        private readonly IConfirmationEmitter _confirmationEmitter;
        private readonly IToDoItemGenerator _toDoItemGenerator;

        private readonly IToDoItemStorage _storage;
        private bool _isDisposed;

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
            var items = await _storage.LoadAllAsync();
            foreach (var item in items)
            {
                ToDoItems.Insert(0, new ToDoItemViewModel(item));
            }
        }

        private async Task DeleteItem(ToDoItemViewModel? todoItem)
        {
            if (todoItem == null)
            {
                return;
            }
            ToDoItems.Remove(todoItem);
            await _storage.RemoveAsync(todoItem.Item);
        }

        public ICommand DeleteItemCommand { get; set; }

        public ICommand OpenOptionsWindowCommand { get; set; }
        private void ShowOptionsWindow()
        {
            var settings = new SettingsWindow(Settings.Instance);
            settings.ShowDialog();
            this.UpdateSettingBasedProperties();
        }

        public void CreateToDoItemFromClipboardContent(object? sender, object args)
        {
            var todoItem = _toDoItemGenerator.GenerateItem();
            if (todoItem == null)
            {
                _confirmationEmitter.NoData();
                return;
            }

            Task.Run(()=>_storage.InsertAsync(todoItem));
            ToDoItems.Insert(0, new ToDoItemViewModel(todoItem));
            _confirmationEmitter.Done();
        }

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
            await _storage.SaveAsync();
        }

        private void UpdateSettingBasedProperties()
        {
            OnPropertyChanged(nameof(ShowCompleted));
        }

        #region Disposable
        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _storage.Dispose();
                }
                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
