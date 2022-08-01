using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoLite.Core;
using ToDoLite.Core.DataModel;
using ToDoLite.Core.Windows;

namespace ToDoLite.App.Windows.ViewModel
{
    public class ToDoItemViewModel : ObservableObject
    {
        private bool _isEditMode;
        private Brush _backColor;
        private Point? _lastMouseButtonDownLocation;

        public ToDoItemViewModel(ToDoItem item)
        {
            Item = item;

            if (Item.CapturedDataType == ClipboardDataType.Image)
            {
                Image = DataConverter.GetImage(item.RawData);
            }
            else if (Item.CapturedDataType == ClipboardDataType.Html)
            {
                TextData = item.PlainText;
            }
            else
            {
                TextData = DataConverter.GetString(item.RawData);
            }

            _ = StartTimestampUpdateLoop();
            SetEditModeCommand = new RelayCommand(() => this.IsEditMode = !this.IsEditMode);
            HandleMouseLeftButtonDownOnImage = new RelayCommand<MouseButtonEventArgs>(StoreMousePositionAtMouseDown);
            HandleMouseLeftButtonUpOnImage = new RelayCommand<MouseButtonEventArgs>(OpenFullSizeImageInWindow);
        }

        private void StoreMousePositionAtMouseDown(MouseButtonEventArgs? e)
        {
            //to allow dragging the image in preview without opening the full size
            _lastMouseButtonDownLocation = GetMousePositionRelativeToImageBorder(e);
        }

        public bool IsCompleted
        {
            get => Item.IsCompleted;
            set
            {
                CompletedDateTime = value ? DateTime.UtcNow : DateTime.MinValue;
                SetProperty(Item.IsCompleted, value, Item, (targetObject, newValue) => targetObject.IsCompleted = newValue);
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

        public ClipboardDataType DataType => Item.CapturedDataType;

        public string CompletedDateTimeFormatted => $"{ Item.CompletedDateTime.ToLocalTime():dddd, dd MMMM HH:mm}";
        public string TimeDifferenceFromCompleted => $"({DateTimeExtensions.GetTimeDifference(Item.CompletedDateTime)})";

        public string? TextData { get; set; }
        public BitmapImage? Image { get; }
        public ToDoItem Item { get; }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                SetProperty(ref _isEditMode, value);
                if (value)
                {
                    BackColor = Brushes.Red;
                }
                else
                {
                    BackColor = Brushes.AntiqueWhite;

                }
            }
        }

        public ICommand SetEditModeCommand { get; set; }
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

        public Brush BackColor
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
        }
    }
}
