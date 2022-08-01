using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ToDoLite.App.Windows.ViewModel;

public class FullSizePreviewWindowViewModel : ObservableObject
{
#pragma warning disable CS8618
    public FullSizePreviewWindowViewModel() { }
#pragma warning restore CS8618

    public FullSizePreviewWindowViewModel(ToDoItemViewModel toDoItemViewModel)
    {
        Title = $"{toDoItemViewModel.ActiveWindowTitle} {toDoItemViewModel.CreatedDateTimeFormatted}";
        Image = toDoItemViewModel.Image;
    }
    public string Title { get; }
    public BitmapImage? Image { get; }
}