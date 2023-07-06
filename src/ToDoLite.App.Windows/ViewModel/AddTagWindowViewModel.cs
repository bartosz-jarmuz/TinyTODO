using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoLite.Core.DataModel;

namespace ToDoLite.App.Windows.ViewModel;

public class AddTagWindowViewModel : ObservableObject
{
    public AddTagWindowViewModel()
    {
        AddNewTagCommand = new RelayCommand(CreateTag);
    }

    private void CreateTag()
    {
        if (TagName != null)
        {
            CloseWindow?.Invoke();
        }
        
    }

    public ICommand AddNewTagCommand { get; private set; }
    public string? TagName { get; set; }
    public string? TagDescription { get; set; }
    public Action? CloseWindow { get; set; }
}