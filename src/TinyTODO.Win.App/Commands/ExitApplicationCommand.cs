using System;
using System.Windows.Input;

namespace TinyTODO.App.Windows.Commands;

class ExitApplicationCommand : ICommand
{
    private readonly MainWindow _mainWindow;

    public ExitApplicationCommand(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
    }

    public void Execute(object? parameter)
    {
        _mainWindow.Dispose();
        App.Current.Shutdown();
    }


    public bool CanExecute(object? parameter) => true;

    public event EventHandler? CanExecuteChanged;
}