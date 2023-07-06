using System.Collections.Concurrent;
using System.Windows.Media;
using Fernandezja.ColorHashSharp;

namespace ToDoLite.App.Windows.TextToColor;

public class TextToColorConverter
{
    private TextToColorConverter()
    {
        _generator = new ColorHash();
        _cache = new ConcurrentDictionary<string, SolidColorBrush>();
    }


    private static readonly Lazy<TextToColorConverter> _instance = new(() => new TextToColorConverter());
    private readonly ColorHash _generator;
    private readonly ConcurrentDictionary<string, SolidColorBrush> _cache;
    public static TextToColorConverter Instance => _instance.Value;

    public Brush GetBrush(string text)
    {
        if (_cache.TryGetValue(text, out var storedColor))
        {
            return storedColor;
        }
        var color = _generator.Rgb(text);
        var brush = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));
        _cache.TryAdd(text, brush);
        return brush;
    }
}
