using System.Windows;
using ToDoLite.App.Windows.ViewModel;

namespace ToDoLite.App.Windows;
/// <summary>
/// Interaction logic for AddTagWindow.xaml
/// </summary>
public partial class AddTagWindow : Window
{
    public AddTagWindow()
    {
        InitializeComponent();
        var vm = new AddTagWindowViewModel();
        DataContext = vm;
        vm.CloseWindow ??= Close;
        NameInputBox.Focus();
    }
}
