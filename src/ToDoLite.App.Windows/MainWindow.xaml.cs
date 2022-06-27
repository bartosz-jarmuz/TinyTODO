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
using Microsoft.Extensions.DependencyInjection;
using ToDoLite.App.Windows.Commands;
using ToDoLite.App.Windows.ViewModel;
using ToDoLite.Core;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;
using ToDoLite.Core.Persistence;
using ToDoLite.Core.Windows;
using Settings = ToDoLite.Core.Persistence.Settings;

namespace ToDoLite.App.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IDisposable
{
    private readonly MainWindowViewModel _viewModel;
    private bool _isDisposed;

    public MainWindow()
    {
        InitializeComponent();
        InitializeTaskbarIcon();
        _viewModel = App.Current.Services.GetService<MainWindowViewModel>()!;
        DataContext = _viewModel;
    }

    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            HotkeyManager.Current.AddOrReplace(HotkeyIdentifiers.StoreClipboardContent, Key.C, ModifierKeys.Shift | ModifierKeys.Alt, _viewModel.CreateToDoItemFromClipboardContent);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to register a hotkey. Perhaps another instance of the app is already running?\r\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        await _viewModel.Initialize();
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
                _viewModel.Dispose();
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
