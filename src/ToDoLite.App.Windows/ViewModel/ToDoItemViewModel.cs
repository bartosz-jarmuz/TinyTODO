using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoLite.Core;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;
using ToDoLite.Core.Windows.DataConversion;

namespace ToDoLite.App.Windows.ViewModel
{
    public class ToDoItemViewModel : ObservableObject
    {
        private readonly ITagRepository _tagRepository;
        private bool _isEditMode;
        private Brush? _backColor;
        private Point? _lastMouseButtonDownLocation;
        private bool _isTextBoxFocused;

        public ToDoItemViewModel(ToDoItem item,  ITagRepository tagRepository)
        {
            _tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
            Item = item;

            if (Item.Images.Any())
            {
                Image = ImageConverter.GetImage(item.Images.First().Bytes);
            }

            Tags = new ObservableCollection<TagViewModel>();
            foreach (var tag in Item.Tags)
            {
                Tags.Add(new TagViewModel(tag));
            }
            TextData = StringConverter.GetString(item.RawData);

            _ = StartTimestampUpdateLoop();
            SetEditModeCommand = new RelayCommand(() => IsEditMode = true);
            SetNonEditModeCommand = new RelayCommand(() => IsEditMode = false);
            AddTagCommand = new RelayCommand(async ()=> await AddTag());
            SaveTextChangeCommand = new RelayCommand(SaveTextChange);
            HandleMouseLeftButtonDownOnImage = new RelayCommand<MouseButtonEventArgs>(StoreMousePositionAtMouseDown);
            HandleMouseLeftButtonUpOnImage = new RelayCommand<MouseButtonEventArgs>(OpenFullSizeImageInWindow);
        }

       
        public event EventHandler? ItemUpdated;

        private void StoreMousePositionAtMouseDown(MouseButtonEventArgs? e)
        {
            //to allow dragging the image in preview without opening the full size
            if (e != null)
            {
                _lastMouseButtonDownLocation = GetMousePositionRelativeToImageBorder(e);
            }
        }

        public bool IsCompleted
        {
            get => Item.IsCompleted;
            set
            {
                CompletedDateTime = value ? DateTime.UtcNow : DateTime.MinValue;
                SetProperty(Item.IsCompleted, value, Item, (targetObject, newValue) => targetObject.IsCompleted = newValue);
                ItemUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public string? ActiveWindowTitle => Item.ActiveWindowTitle;
        public string CreatedDateTimeFormatted => $"{Item.CreatedDateTime.ToLocalTime():dddd, dd MMMM HH:mm}";
        public string TimeDifferenceFromCreated => $"({DateTimeExtensions.GetTimeDifference(Item.CreatedDateTime)})";
        
        private DateTime CompletedDateTime
        {
            get => Item.CompletedDateTime;
            set
            {
                SetProperty(Item.CompletedDateTime, value, Item, (targetObject, newValue) => targetObject.CompletedDateTime = newValue);
                OnPropertyChanged(nameof(TimeDifferenceFromCompleted));
                OnPropertyChanged(nameof(CompletedDateTimeFormatted));
            }
        }

        private async Task AddTag()
        {
            var window = new AddTagWindow();
            window.ShowDialog();
            window.Activate();
            window.Focus();
            if (window.DataContext is AddTagWindowViewModel vm && vm.ShouldSaveNewTag == true)
            {
                if (vm.TagName != null)
                {
                    var tag = await _tagRepository.GetOrCreateTagAsync(vm.TagName, vm.TagDescription);
                    if (Item.Tags.All(x => x.Id != tag.Id))
                    {
                        Item.Tags.Add(tag);
                        ItemUpdated?.Invoke(this, EventArgs.Empty);
                        Tags.Add(new TagViewModel(tag));
                    }
                }
            }
        }

        private void SaveTextChange()
        {
            if (!IsEditMode)
            {
                return;
            }

            Item.PlainText = RtfConverter.ConvertToPlainText(TextData);
            
            SetProperty(Item.RawData, StringConverter.GetBytes(TextData), Item, (targetObject, newValue) => targetObject.RawData = newValue);
          
            IsEditMode = false;
            ItemUpdated?.Invoke(this, EventArgs.Empty);
        }

        public string CompletedDateTimeFormatted => $"{ Item.CompletedDateTime.ToLocalTime():dddd, dd MMMM HH:mm}";
        public string TimeDifferenceFromCompleted => $"({DateTimeExtensions.GetTimeDifference(Item.CompletedDateTime)})";

        public string? TextData { get; set; }
        public BitmapImage? Image { get; }
        public ToDoItem Item { get; }

        public ObservableCollection<TagViewModel> Tags { get; }

        public bool IsTextBoxFocused
        {
            get => _isTextBoxFocused;
            set => SetProperty(ref _isTextBoxFocused, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                SetProperty(ref _isEditMode, value);
                if (value)
                {
                    IsTextBoxFocused = true;
                    BackColor = Brushes.Beige;
                }
                else
                {
                    Keyboard.ClearFocus();
                    IsTextBoxFocused = false;
                    BackColor = null;
                }
            }
        }

        public ICommand SetEditModeCommand { get; set; }
        public ICommand SetNonEditModeCommand { get; set; }
        public ICommand AddTagCommand { get; set; }
        public ICommand SaveTextChangeCommand { get; set; }
        public ICommand HandleMouseLeftButtonUpOnImage { get; set; }
        public ICommand HandleMouseLeftButtonDownOnImage { get; set; }

        private void OpenFullSizeImageInWindow(MouseButtonEventArgs? e)
        {
            if (e != null)
            {
                if (_lastMouseButtonDownLocation == GetMousePositionRelativeToImageBorder(e))
                {
                    var window = new FullSizePreviewWindow
                    {
                        DataContext = new FullSizePreviewWindowViewModel(this)
                    };
                    window.Show();
                }
            }
        }

        private static Point GetMousePositionRelativeToImageBorder(MouseButtonEventArgs e)
        {
            return e.GetPosition((e.Source as Image)?.Parent as UIElement);
        }

        public Brush? BackColor
        {
            get => _backColor;
            set => SetProperty(ref _backColor, value);
        }

        private async Task StartTimestampUpdateLoop()
        {
            while (true)
            {
                OnPropertyChanged(nameof(TimeDifferenceFromCreated));
                await Task.Delay(CalculateDelay());
            }
            TimeSpan CalculateDelay()
            {
                var diff = DateTime.UtcNow - Item.CreatedDateTime;
                if (diff <= TimeSpan.FromSeconds(90))
                {
                    return TimeSpan.FromSeconds(1);
                } else if (diff <= TimeSpan.FromMinutes(60))
                {
                    return TimeSpan.FromMinutes(1);
                }
                else
                {
                    return TimeSpan.FromHours(1);
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
