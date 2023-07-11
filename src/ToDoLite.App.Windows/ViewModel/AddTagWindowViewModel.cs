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
        AddNewTagCommand = new RelayCommand(ConfirmTagCreation);
        CancelCommand = new RelayCommand(() => CloseWindow?.Invoke());
    }

    private void ConfirmTagCreation()
    {
        if (TagName != null)
        {
            ShouldSaveNewTag = true;
            CloseWindow?.Invoke();
        }
        
    }

    public bool ShouldSaveNewTag { get; private set; }
    public ICommand AddNewTagCommand { get; private set; }
    public ICommand CancelCommand { get; private set; }
    public string? TagName { get; set; }
    public string? TagDescription { get; set; }
    public Action? CloseWindow { get; set; }
}