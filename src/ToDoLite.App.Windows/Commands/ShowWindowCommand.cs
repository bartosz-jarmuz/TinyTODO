using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ToDoLite.App.Windows.Commands
{
    class ShowWindowCommand : ICommand
    {
        private readonly MainWindow _mainWindow;

        public ShowWindowCommand(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void Execute(object? parameter)
        {
            _mainWindow.Show();
            SystemCommands.RestoreWindow(_mainWindow);
            _mainWindow.Activate();
        }


        public bool CanExecute(object? parameter) => true;

#pragma warning disable 67
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 67
    }
}
