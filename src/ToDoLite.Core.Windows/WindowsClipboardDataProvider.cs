using System.Windows;
using ToDoLite.Core.ClipboardModel;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;
using ToDoLite.Core.Windows.DataConversion;

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
        if (dataObject.GetDataPresent(DataFormats.Html))
        {
            //at the moment, cannot use HTML data properly, because I have no way to convert HTML markup (what you get by line below) to RTF (our text boxes are all RTF because of how we want to allow editing)
            //var data = dataObject.GetData(DataFormats.Html);
            //therefore, as a workaround, do not store HTML data at all and rather convert plain text to RTF and store that data.
            //this is because conversion of plain text to RTF preserves whitespace etc, so all in all the ToDoItem looks okay(ish)
            var formattedPlainTextAsWorkaroundForHtml = RtfConverter.ConvertToRtf(plainText);

            return new TextualClipboardData(CapturedDataType.Html, plainText, StringConverter.GetBytes(formattedPlainTextAsWorkaroundForHtml));
        }
        else if (dataObject.GetDataPresent(DataFormats.Rtf))
        {
            var data = dataObject.GetData(DataFormats.Rtf);
            return new TextualClipboardData(CapturedDataType.RichText, plainText, StringConverter.GetBytes(data?.ToString() ?? ""));
        }
        else if (Clipboard.ContainsImage())
        {
            var data = Clipboard.GetImage();
            if (data != null)
            {
                return new ImageClipboardData(ImageConverter.GetBytes(data));
            }
            else
            {
                throw new InvalidOperationException($"Clipboard data is supposed to be an image but the type is not expected: [{dataObject.GetType()}");
            }
        }
        else if (Clipboard.ContainsText())
        {
            return new TextualClipboardData(CapturedDataType.PlainText, plainText, StringConverter.GetBytes(plainText));
        }
        else
        {
            return new TextualClipboardData(CapturedDataType.Unsupported, $"Unsupported format: [{string.Join(", ", Clipboard.GetDataObject()?.GetFormats() ?? new[]{"Data Object or format are null"})}]", Array.Empty<byte>());
        }
    }

    
}
