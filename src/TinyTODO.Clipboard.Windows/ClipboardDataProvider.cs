using System.Windows;
using TinyTODO.Core.DataModel;

namespace TinyTODO.Core.Windows;

public partial class ClipboardDataProvider
{
    public ClipboardData? GetData()
    {
        DataObject? data = System.Windows.Clipboard.GetDataObject() as DataObject;
        if (data == null)
        {
            return null;
        }

        if (data.GetDataPresent(DataFormats.Html))
        {
            return new ClipboardData(ClipboardDataType.Html, data.GetData(DataFormats.Html));
        }
        else if (data.GetDataPresent(DataFormats.Rtf))
        {
            return new ClipboardData(ClipboardDataType.RichText, data.GetData(DataFormats.Rtf));
        }
        else if (Clipboard.ContainsImage())
        {
            return new ClipboardData(ClipboardDataType.Image, Clipboard.GetImage());
        }
        else if (Clipboard.ContainsText())
        {
            return new ClipboardData(ClipboardDataType.PlainText, Clipboard.GetText());
        }
        else
        {
            return new ClipboardData(ClipboardDataType.Unsupported, $"Unsupported format: [{string.Join(", ", Clipboard.GetDataObject().GetFormats())}]");
        }
    }
}
