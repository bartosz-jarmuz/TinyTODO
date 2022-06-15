using System;
using System.Windows.Input;

namespace ToDoLite.App.Windows.Commands;

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

#pragma warning disable 67
    public event EventHandler? CanExecuteChanged;
#pragma warning restore 67
}