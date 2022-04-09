using System;
using TinyTODO.Core.Contracts;

namespace TinyTODO.Core.Windows;

public class ConsoleBeepEmitter : IConfirmationEmitter
{
    public void Done()
    {
        Console.Beep(500, 150);
    }

    public void NoData()
    {
        Console.Beep(200, 400);
    }
}
