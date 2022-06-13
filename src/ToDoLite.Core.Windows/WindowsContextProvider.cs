using System.Runtime.InteropServices;
using System.Text;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;

namespace ToDoLite.Core;

public class WindowsContextProvider : IContextProvider
{
    public ItemCreationContext GetToDoContext()
    {
        const int nChars = 256;
        StringBuilder Buff = new StringBuilder(nChars);
        IntPtr handle = GetForegroundWindow();
        string? windowTitle = null;
        if (GetWindowText(handle, Buff, nChars) > 0)
        {
            windowTitle = Buff.ToString();
        }

        return new ItemCreationContext(windowTitle);
    }

    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("User32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static public extern IntPtr GetForegroundWindow();
}
