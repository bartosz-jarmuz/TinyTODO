using System;
using System.Windows;
using System.Windows.Controls;
using Hardcodet.Wpf.TaskbarNotification;
using ToDoLite.App.Windows.Commands;

namespace ToDoLite.App.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IDisposable
{
    private TaskbarIcon _taskbarIcon = null!;

    private void InitializeTaskbarIcon()
    {
        _taskbarIcon = (TaskbarIcon)FindResource("MainTaskbarIcon");

        _taskbarIcon.DoubleClickCommand = new ShowWindowCommand(this);
        _taskbarIcon.ContextMenu = new ContextMenu();
        var closeButton = new MenuItem
        {
            Command = new ExitApplicationCommand(this),
            Header = "Exit"
        };
        _taskbarIcon.ContextMenu.Items.Add(closeButton);
    }
}
