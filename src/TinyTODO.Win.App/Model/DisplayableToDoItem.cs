using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TinyTODO.Core.DataModel;
using TinyTODO.Core.Windows;

namespace TinyTODO.App.Windows.Model
{
    public class DisplayableToDoItem
    {
        public DisplayableToDoItem(ToDoItem item)
        {
            this.PlainTextData = item.PlainTextData;
            this.DataType = item.DataType;
            if (this.DataType == ClipboardDataType.Image)
            {
                this.Image = DataConverter.GetImage(item.RawData);
            }
            else
            {
                this.TextData = DataConverter.GetString(item.RawData);
            }

            this.ActiveWindowTitle = item.ActiveWindowTitle;
            this.CreatedDateTime = item.CreatedDateTime;
            this.CompletedDateTime = item.CompletedDateTime;
            this.IsCompleted= item.IsCompleted;
            this.Id = item.Id;
        }

        public Guid Id { get; set; }
        public bool IsCompleted { get; set; }
        public ClipboardDataType DataType { get; set; }
        public string? ActiveWindowTitle { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime CompletedDateTime { get; set; }
        public string? PlainTextData { get; set; }
        public string? TextData { get; set; }
        public BitmapImage? Image { get; set; }
    }
}
