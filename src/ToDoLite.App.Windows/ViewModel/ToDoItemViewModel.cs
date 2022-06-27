using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using ToDoLite.Core;
using ToDoLite.Core.DataModel;
using ToDoLite.Core.Windows;

namespace ToDoLite.App.Windows.ViewModel
{
    public class ToDoItemViewModel : ObservableObject
    {
        public ToDoItemViewModel(ToDoItem item)
        {
            Item = item;

            if (Item.DataType == ClipboardDataType.Image)
            {
                Image = DataConverter.GetImage(item.RawData);
            }
            else
            {
                TextData = DataConverter.GetString(item.RawData);
            }
            _ = StartTimestampUpdateLoop();
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
        public string CreatedDateTimeFormatted => $"{Item.CreatedDateTime.ToLocalTime().ToString("dddd, dd MMMM HH:mm")}";
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

        public string CompletedDateTimeFormatted => $"{ Item.CompletedDateTime.ToLocalTime().ToString("dddd, dd MMMM HH:mm")}";
        public string TimeDifferenceFromCompleted => $"({DateTimeExtensions.GetTimeDifference(Item.CompletedDateTime)})";

        public string? PlainTextData => Item.PlainTextData;
        public string? TextData { get; }
        public BitmapImage? Image { get; }
        public ToDoItem Item { get; }

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
