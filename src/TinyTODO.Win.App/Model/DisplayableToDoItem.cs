using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TinyTODO.Core.DataModel;
using TinyTODO.Core.Windows;

namespace TinyTODO.App.Windows.Model
{
    public class DisplayableToDoItem : INotifyPropertyChanged
    {
        private bool isCompleted;

        public DisplayableToDoItem(ToDoItem item)
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
        }

        public Guid Id { get; set; }
        public bool IsCompleted
        {
            get => isCompleted; 
            set
            {
                isCompleted = value;
                Item.IsCompleted = value;
                OnPropertyChanged();
            }
        }
        public ClipboardDataType DataType { get; set; }
        public string? ActiveWindowTitle { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime CompletedDateTime { get; set; }
        public string? PlainTextData { get; set; }
        public string? TextData { get; set; }
        public BitmapImage? Image { get; set; }
        public ToDoItem Item { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
