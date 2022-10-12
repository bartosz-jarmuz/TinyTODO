using System.Windows;
using System.Windows.Media.Imaging;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Windows;

public class WindowsClipboardDataProvider : IClipboardDataProvider
{
    public ClipboardData? GetData()
    {
        DataObject? dataObject = Clipboard.GetDataObject() as DataObject;
        if (dataObject == null)
        {
            return null;
        }
        var plainText = Clipboard.GetText();
        plainText = DataConverter.ConvertToRtf(plainText);
        if (dataObject.GetDataPresent(DataFormats.Html))
        {
            var data = dataObject.GetData(DataFormats.Html);
            return new ClipboardData(ClipboardDataType.Html, plainText, DataConverter.GetBytes(data?.ToString()??""));
        }
        else if (dataObject.GetDataPresent(DataFormats.Rtf))
        {
            var data = dataObject.GetData(DataFormats.Rtf);
            return new ClipboardData(ClipboardDataType.RichText, plainText, DataConverter.GetBytes(data?.ToString() ?? ""));
        }
        else if (Clipboard.ContainsImage())
        {
            var data = Clipboard.GetImage();
            if (data is BitmapSource bitmapSource)
            {
                return new ClipboardData(ClipboardDataType.Image, plainText, DataConverter.GetBytes(bitmapSource));
            }
            else
            {
                throw new InvalidOperationException($"Clipboard data is supposed to be an image but the type is not expected: [{dataObject.GetType()}");
            }
        }
        else if (Clipboard.ContainsText())
        {
            return new ClipboardData(ClipboardDataType.PlainText, plainText, DataConverter.GetBytes(plainText));
        }
        else
        {
            return new ClipboardData(ClipboardDataType.Unsupported, $"Unsupported format: [{string.Join(", ", Clipboard.GetDataObject().GetFormats())}]", Array.Empty<byte>());
        }
    }

    
}
