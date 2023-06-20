using System.Text;

namespace ToDoLite.Core.Windows.DataConversion;

public static class StringConverter
{
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
}