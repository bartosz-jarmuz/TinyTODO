using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ToDoLite.Core;
using ToDoLite.Core.DataModel;
using ToDoLite.Core.Windows;

namespace ToDoLite.App.Windows.ViewModel
{
    public class ToDoItemViewModel : INotifyPropertyChanged
    {
        private bool _isCompleted;

        public ToDoItemViewModel(ToDoItem item)
        {
            Item = item;

            PlainTextData = item.PlainTextData;
            DataType = item.DataType;
            if (DataType == ClipboardDataType.Image)
            {
                Image = DataConverter.GetImage(item.RawData);
            }
            else
            {
                TextData = DataConverter.GetString(item.RawData);
            }

            ActiveWindowTitle = item.ActiveWindowTitle;
            CreatedDateTime = item.CreatedDateTime;
            CompletedDateTime = item.CompletedDateTime;
            IsCompleted = item.IsCompleted;
            Id = item.Id;
            _ = UpdateTimestamp();
        }

        public Guid Id { get; set; }

        public bool IsCompleted
        {
            get => _isCompleted; 
            set
            {
                _isCompleted = value;
                Item.IsCompleted = value;
                OnPropertyChanged();
            }
        }
        public ClipboardDataType DataType { get; set; }
        public string? ActiveWindowTitle { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedDateTimeFormatted => $"{CreatedDateTime.ToLocalTime().ToString("dddd, dd MMMM HH:mm")}";
        public string TimeDifference => $"({DateTimeExtensions.GetTimeDifference(CreatedDateTime)})";
        public DateTime CompletedDateTime { get; set; }
        public string? PlainTextData { get; set; }
        public string? TextData { get; set; }
        public BitmapImage? Image { get; set; }
        public ToDoItem Item { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private async Task UpdateTimestamp()
        {
            while (true)
            {
                OnPropertyChanged(nameof(TimeDifference));
                await Task.Delay(CalculateDelay());
            }
            TimeSpan CalculateDelay()
            {
                var diff = DateTime.UtcNow - CreatedDateTime;
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

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
