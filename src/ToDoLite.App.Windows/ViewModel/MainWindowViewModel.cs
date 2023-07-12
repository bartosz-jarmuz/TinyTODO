using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;
using Settings = ToDoLite.Core.Persistence.Settings;

namespace ToDoLite.App.Windows.ViewModel
{
    public class MainWindowViewModel : ObservableObject, IDisposable
    {
#pragma warning disable CS8618
        public MainWindowViewModel() { }
#pragma warning restore CS8618

        public MainWindowViewModel(IToDoItemStorage toDoItemStorage, ITagRepository tagRepository, IToDoItemGenerator toDoItemGenerator, IConfirmationEmitter confirmationEmitter, IDataExporter dataExporter)
        {
            _storage = toDoItemStorage;
            _tagRepository = tagRepository;
            _toDoItemGenerator = toDoItemGenerator;
            _confirmationEmitter = confirmationEmitter;
            _dataExporter = dataExporter;
            ToDoItems = new ObservableCollection<ToDoItemViewModel>();
            AvailableTags = new ObservableCollection<TagViewModel>();
            ToDoItemsCollectionView = CollectionViewSource.GetDefaultView(ToDoItems);
            ToDoItemsCollectionView.Filter = (item) => CurrentTagFilter == null ? true : ((ToDoItemViewModel)item).Tags.Any(x => x.Name == CurrentTagFilter.Name);
            ToDoItems.CollectionChanged += OnToDoItemsCollectionChange;
            DeleteItemCommand = new AsyncRelayCommand<ToDoItemViewModel>(DeleteItem);
            OpenOptionsWindowCommand = new RelayCommand(ShowOptionsWindow);
            OpenAddNewItemWindowCommand = new RelayCommand(() => OpenAddNewItemWindow());
            ExportDataCommand = new AsyncRelayCommand(ExportDataAsync);
            ImportDataCommand = new AsyncRelayCommand(ImportDataAsync);
            ClearTagFilterCommand = new RelayCommand(() => CurrentTagFilter = null);

        }

        private readonly IConfirmationEmitter _confirmationEmitter;
        private readonly IDataExporter _dataExporter;
        private readonly IToDoItemGenerator _toDoItemGenerator;

        private readonly IToDoItemStorage _storage;
        private readonly ITagRepository _tagRepository;
        private bool _isDisposed;
        private TagViewModel? currentTagFilter;

        public bool ShowCompleted
        {
            get => Settings.Instance.ShowCompleted;
            set
            {
                Settings.Instance.ShowCompleted = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TagViewModel> AvailableTags { get; set; }

        public TagViewModel? CurrentTagFilter
        {
            get => currentTagFilter;
            set
            {
                currentTagFilter = value;
                ToDoItemsCollectionView.Refresh();
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ToDoItemViewModel> ToDoItems { get; set; }

        public ICollectionView ToDoItemsCollectionView { get; }

        public async Task InitializeAsync()
        {
            try
            {
                await LoadItemsAsync();
                await LoadTagsAsync();
            }
            catch (Exception e)
            {
                if (MessageBox.Show($"An error occurred when trying to load database:\r\n{e.Message}\r\nWould you like to recreate the database from scratch?\r\n(All data will be lost!)", "Database initialization error", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                {
                    await _storage.RecreateDatabaseAsync();
                    await LoadItemsAsync();
                }
                else
                {
                    throw;
                }
            }


            async Task LoadItemsAsync()
            {
                var items = await _storage.LoadAllAsync();
                foreach (var item in items.OrderBy(x => x.CreatedDateTime))
                {
                    ToDoItems.Insert(0, new ToDoItemViewModel(item, _tagRepository));
                }
            }

            async Task LoadTagsAsync()
            {
                var tags = await _tagRepository.LoadAllUsedTagsAsync();
                foreach (var tag in tags)
                {
                    AvailableTags.Add(new TagViewModel(tag));
                }
                _tagRepository.TagAssigned += _tagRepository_TagAssigned;
            }
        }

        private void _tagRepository_TagAssigned(object? sender, TagAssignedEventArgs e)
        {
            var existingTag = AvailableTags.FirstOrDefault(t => e.Tag.Name == t.Name);
            if (existingTag == null)
            {
                AvailableTags.Add(new TagViewModel(e.Tag));
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
        public ICommand ExportDataCommand { get; set; }
        public ICommand ImportDataCommand { get; set; }
        public ICommand OpenAddNewItemWindowCommand { get; set; }
        public ICommand ClearTagFilterCommand { get; set; }

        private void ShowOptionsWindow()
        {
            var settings = new SettingsWindow(Settings.Instance);
            settings.ShowDialog();
            this.UpdateSettingBasedProperties();
        } 
        
        private async Task ExportDataAsync()
        {
            var data = await _storage.LoadAllAsync();

            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} ToDoLite DataExport.json");
                _dataExporter.ExportData(data, path);
                MessageBox.Show(path, "Data exported successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }   
        
        private async Task ImportDataAsync()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {

                    var items = _dataExporter.ImportData(openFileDialog.FileName);
                    foreach (var item in items)
                    {
                        await _storage.InsertAsync(item);
                        ToDoItems.Insert(0, new ToDoItemViewModel(item));
                    }
                    MessageBox.Show($"Loaded {items.Count} items.", "Data imported successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Import Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        } 
        
        public void OpenAddNewItemWindow(object? _ = null, object? __ = null)
        {
            var window = new AddItemWindow();
            window.ShowDialog();
            window.Activate();
            window.Focus();
            if (window.DataContext is AddItemWindowViewModel addItemViewModel)
            {
                if (addItemViewModel.ToDoItem != null)
                {
                    Task.Run(() => _storage.InsertAsync(addItemViewModel.ToDoItem));
                    ToDoItems.Insert(0, new ToDoItemViewModel(addItemViewModel.ToDoItem, _tagRepository));
                    _confirmationEmitter.Done();
                }
            }
        }

        public void CreateToDoItemFromClipboardContent(object? _, object __)
        {
            var todoItem = _toDoItemGenerator.GenerateItem();
            if (todoItem == null)
            {
                _confirmationEmitter.NoData();
                return;
            }

            Task.Run(() => _storage.InsertAsync(todoItem));
            ToDoItems.Insert(0, new ToDoItemViewModel(todoItem, _tagRepository));
            _confirmationEmitter.Done();
        }

        void OnToDoItemsCollectionChange(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (ToDoItemViewModel item in e.NewItems)
                    item.ItemUpdated += OnToDoItemPropertyChanged;

            if (e.OldItems != null)
                foreach (ToDoItemViewModel item in e.OldItems)
                    item.ItemUpdated -= OnToDoItemPropertyChanged;
        }

        async void OnToDoItemPropertyChanged(object? sender, EventArgs eventArgs)
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
