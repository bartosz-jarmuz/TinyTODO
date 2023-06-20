using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ToDoLite.Core.Windows.DataConversion;

public static class RtfConverter
{
    public static string ConvertToRtf(string value)
    {
        RichTextBox rtb = new RichTextBox();
        rtb.AppendText(value);
        TextRange textRange = new TextRange(
            rtb.Document.ContentStart,
            rtb.Document.ContentEnd
        );
        using MemoryStream ms = new MemoryStream();
        textRange.Save(ms, DataFormats.Rtf);
        string rtf = Encoding.Default.GetString(ms.ToArray());
        int offset = rtf.IndexOf(@"\f0\fs17", StringComparison.OrdinalIgnoreCase) + 8; // offset = 118;
        int len = rtf.LastIndexOf(@"\par", StringComparison.OrdinalIgnoreCase) - offset;
        string _ = rtf.Substring(offset, len).Trim();
        return rtf;
    }

    public static string ConvertToPlainText(string? rtf)
    {
        var flowDocument = new FlowDocument();
        var textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);

        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rtf ?? string.Empty)))
        {
            textRange.Load(stream, DataFormats.Rtf);
        }

        return textRange.Text;
    }
}