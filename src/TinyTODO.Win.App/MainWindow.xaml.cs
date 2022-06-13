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
using TinyTODO.App.Windows.Commands;
using TinyTODO.App.Windows.Model;
using TinyTODO.App.Windows.ViewModel;
using TinyTODO.Core;
using TinyTODO.Core.Contracts;
using TinyTODO.Core.DataModel;
using TinyTODO.Core.Windows;

namespace TinyTODO.App.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IDisposable
{
    private IConfirmationEmitter _confirmationEmitter = new ConsoleBeepEmitter();
    private IClipboardDataProvider _clipboardProvider = new WindowsClipboardDataProvider();
    private IToDoItemStorage _storage = new ToDoItemStorage();
    private IContextProvider _contextProvider = new WindowsContextProvider();
    private MainWindowViewModel _viewModel;
    private bool _isDisposed;
    private readonly TaskbarIcon _taskbarIcon;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainWindowViewModel();
        _viewModel.Storage = _storage;
        DataContext = _viewModel;

        HotkeyManager.Current.AddOrReplace(HotkeyIdentifiers.StoreClipboardContent, Key.C, ModifierKeys.Shift | ModifierKeys.Alt, (sender, args) => OnHotkey(sender, args));

        _taskbarIcon = (TaskbarIcon)FindResource("MainTaskbarIcon");
        InitializeTaskbarIcon();
    }

    private void InitializeTaskbarIcon()
    {
        _taskbarIcon.DoubleClickCommand = new ShowWindowCommand(this);
        _taskbarIcon.ContextMenu = new ContextMenu();
        var closeButton = new MenuItem();
        closeButton.Command = new ExitApplicationCommand(this);
        closeButton.Header = "Exit";
        _taskbarIcon.ContextMenu.Items.Add(closeButton);
    }


    private void Window_Loaded_1(object sender, RoutedEventArgs e)
    {
        var items = Task.Run(() =>_storage.LoadAllAsync()).Result;
        foreach (var item in items)
        {
            _viewModel.Items.Insert(0, new DisplayableToDoItem(item));
        }
    }

    protected override void OnStateChanged(EventArgs e)
    {
        if (WindowState == WindowState.Minimized && Settings.Instance.MinimizeToTray)
            this.Hide();

        base.OnStateChanged(e);
    }

    public void OnHotkey(object? sender, object args)
    {
        ClipboardData? data = _clipboardProvider.GetData();
        ItemCreationContext? context = _contextProvider.GetToDoContext();
        if (data == null)
        {
            _confirmationEmitter.NoData();
            return;
        }

        ToDoItem? todoItem = new ToDoItem(data, context);

        _storage.InsertAsync(todoItem);
        _viewModel.Add(todoItem);

        _confirmationEmitter.Done();
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        if (Settings.Instance.CloseToTray)
        {
            e.Cancel = true;
            this.Hide();
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _storage.Dispose();
                _taskbarIcon.Dispose();
            }
            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        var settings = new SettingsWindow(Settings.Instance);
        settings.ShowDialog();
        _viewModel.UpdateSettingBasedProperties();
    }
}
