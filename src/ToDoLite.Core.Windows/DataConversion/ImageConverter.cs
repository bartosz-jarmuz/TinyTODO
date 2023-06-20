using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace ToDoLite.Core.Windows.DataConversion;

public static class ImageConverter
{
    public static byte[] GetBytes(BitmapSource source)
    {
        using MemoryStream outStream = new MemoryStream();
        
        BitmapEncoder enc = new BmpBitmapEncoder();
        enc.Frames.Add(BitmapFrame.Create(source));
        enc.Save(outStream);
        var bitmap = new Bitmap(outStream);
        
        using var stream = new MemoryStream();
        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
        return stream.ToArray();
    }

    public static BitmapImage GetImage(byte[]? imageData)
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
}