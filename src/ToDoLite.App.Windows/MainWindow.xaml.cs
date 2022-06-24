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
    private readonly IToDoItemStorage _storage = new ToDoItemStorage();
    private readonly MainWindowViewModel _viewModel;
    private bool _isDisposed;
    private readonly TaskbarIcon _taskbarIcon;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainWindowViewModel(_storage);
        DataContext = _viewModel;

        _taskbarIcon = (TaskbarIcon)FindResource("MainTaskbarIcon");
        InitializeTaskbarIcon();
    }

    private void InitializeTaskbarIcon()
    {
        _taskbarIcon.DoubleClickCommand = new ShowWindowCommand(this);
        _taskbarIcon.ContextMenu = new ContextMenu();
        var closeButton = new MenuItem
        {
            Command = new ExitApplicationCommand(this),
            Header = "Exit"
        };
        _taskbarIcon.ContextMenu.Items.Add(closeButton);
    }

    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            HotkeyManager.Current.AddOrReplace(HotkeyIdentifiers.StoreClipboardContent, Key.C, ModifierKeys.Shift | ModifierKeys.Alt, _viewModel.OnHotkeyPressed);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to register a hotkey. Perhaps another instance of the app is already running?\r\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        await _viewModel.Initialize();
    }

    private void ShowOptionsMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        var settings = new SettingsWindow(Settings.Instance);
        settings.ShowDialog();
        _viewModel.UpdateSettingBasedProperties();
    }

    protected override void OnStateChanged(EventArgs e)
    {
        if (WindowState == WindowState.Minimized && Settings.Instance.MinimizeToTray)
            this.Hide();

        base.OnStateChanged(e);
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        if (Settings.Instance.CloseToTray)
        {
            e.Cancel = true;
            this.Hide();
        }
    }

    #region Disposable
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

#endregion
}
