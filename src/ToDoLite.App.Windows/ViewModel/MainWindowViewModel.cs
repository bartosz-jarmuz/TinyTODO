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
            FilteringTags = new ObservableCollection<TagFilterViewModel>();
            ToDoItemsCollectionView = CollectionViewSource.GetDefaultView(ToDoItems);
            ToDoItemsCollectionView.Filter = (item) => CurrentTagFilter == null ? true : ((ToDoItemViewModel)item).TagViewModels.Any(x => x.Name == CurrentTagFilter.Name);
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

        public ObservableCollection<TagFilterViewModel> FilteringTags { get; set; }

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

        public ObservableCollection<ToDoItemViewModel> ToDoItems { get; set; }

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
                    var usage = tag.ToDoItems.Count(x => !x.IsCompleted);
                    FilteringTags.Add(new TagFilterViewModel(tag, usage));
                }
            }
        }

        private void OnTagAssigned(object? sender, TagAssignedEventArgs e)
        {
            var existingTag = FilteringTags.FirstOrDefault(t => e.Tag.Name == t.Name);
            if (existingTag == null)
            {
                FilteringTags.Add(new TagFilterViewModel(e.Tag, 1));
            }
            else
            {
                UpdateFilteringTagUsageCount(e.Tag);
            }
        }

        private void UpdateFilteringTagUsageCount(Tag tag)
        {
            var existingTag = FilteringTags.FirstOrDefault(t => tag.Name == t.Name);
            if (existingTag == null)
            {
                FilteringTags.Add(new TagFilterViewModel(tag, 1));
            }
            else
            {
                existingTag.UsageCount = existingTag.Tag.ToDoItems.Count(i => !i.IsCompleted);
            }
        }

        private async Task DeleteItem(ToDoItemViewModel? toDoItem)
        {
            if (toDoItem == null)
            {
                return;
            }
            foreach (var tagViewModel in toDoItem.TagViewModels)
            {
                tagViewModel.Tag.ToDoItems.Remove(toDoItem.Item);
                UpdateFilteringTagUsageCount(tagViewModel.Tag);
            }


            ToDoItems.Remove(toDoItem);
           
            await _storage.RemoveAsync(toDoItem.Item);
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
                        ToDoItems.Insert(0, new ToDoItemViewModel(item, _tagRepository));
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
                    AddToDoItem(addItemViewModel.ToDoItem);
                }
            }
        }

        public void CreateToDoItemFromClipboardContent(object? _, object? __)
        {
            var toDoItem = _toDoItemGenerator.GenerateItem();
            AddToDoItem(toDoItem);
        }

        private void AddToDoItem(ToDoItem? toDoItem)
        {
            if (toDoItem == null)
            {
                _confirmationEmitter.NoData();
            }
            else
            {
                Task.Run(() => _storage.InsertAsync(toDoItem));
                ToDoItems.Insert(0, new ToDoItemViewModel(toDoItem, _tagRepository));
                _confirmationEmitter.Done();
            }
            
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

        //tags are :
        // - created from a todo item (new tag)
        // - reused - assigned existing tag to new item (tag usage count increases)
        // - deleted explicitly from a tag view (which means they are removed from filter view and all todo items)
        // - updated when an item is deleted (tag usage count decreases)
        // - updated when an item is completed or un-completed (tag usage count decreases or increases)
        // - updated when a tag is removed (unassigned) from an item

        async void OnToDoItemPropertyChanged(object? sender, EventArgs eventArgs)
        {
            await _storage.SaveAsync();
            var toDoItemViewModel = sender as ToDoItemViewModel;
            if (toDoItemViewModel == null)
                return;
            if (eventArgs is TagAssignedEventArgs tagAssigned)
            {
                OnTagAssigned(sender, tagAssigned);
            } else if (eventArgs is ItemCompletedStateChangedEventArgs)
            {
                foreach (var tagVm in toDoItemViewModel.TagViewModels)
                {
                    UpdateFilteringTagUsageCount(tagVm.Tag);
                }
            }
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
