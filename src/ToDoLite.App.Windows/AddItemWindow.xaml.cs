using System.Windows;
using ToDoLite.App.Windows.ViewModel;

namespace ToDoLite.App.Windows;
/// <summary>
/// Interaction logic for AddItemWindow.xaml
/// </summary>
public partial class AddItemWindow : Window
{
    public AddItemWindow()
    {
        InitializeComponent();
        var vm = new AddItemWindowViewModel();
        DataContext = vm;
        vm.CloseWindow ??= Close;
        InputBox.Focus();
    }
}
