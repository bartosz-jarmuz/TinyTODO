using System;
using NHotkey.Wpf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;
using ToDoLite.App.Windows.Commands;
using ToDoLite.App.Windows.ViewModel;
using ToDoLite.Core;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;
using ToDoLite.Core.Windows;

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
