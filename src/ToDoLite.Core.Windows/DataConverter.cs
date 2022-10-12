

using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace ToDoLite.Core.Windows;

public static class DataConverter
{
    public static byte[] GetBytes(BitmapSource source)
    {
        Bitmap bitmap;
        using (MemoryStream outStream = new MemoryStream())
        {
            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(source));
            enc.Save(outStream);
            bitmap = new System.Drawing.Bitmap(outStream);
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                return stream.ToArray();
            }
        }
    }

    public static BitmapImage GetImage(byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0) 
            return new BitmapImage();

        var image = new BitmapImage();
        using (var mem = new MemoryStream(imageData))
        {
            mem.Position = 0;
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = mem;
            image.EndInit();
        }
        image.Freeze();
        return image;
    }

    public static byte[] GetBytes(string? input)
    {
        if (input == null)
        {
            return Array.Empty<byte>();
        }
        return Encoding.UTF8.GetBytes(input);
    }

    public static string GetString(byte[] input)
    {
        return Encoding.UTF8.GetString(input);
    }

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
        string rtf = ASCIIEncoding.Default.GetString(ms.ToArray());
        int offset = rtf.IndexOf(@"\f0\fs17") + 8; // offset = 118;
        int len = rtf.LastIndexOf(@"\par") - offset;
        string result = rtf.Substring(offset, len).Trim();
        return rtf;
    }
}
