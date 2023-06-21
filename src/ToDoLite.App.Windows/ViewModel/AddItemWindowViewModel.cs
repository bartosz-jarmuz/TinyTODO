using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoLite.Core.ClipboardModel;
using ToDoLite.Core.DataModel;
using ToDoLite.Core.Windows.DataConversion;

namespace ToDoLite.App.Windows.ViewModel;

public class AddItemWindowViewModel : ObservableObject
{
    public AddItemWindowViewModel()
    {
        AddNewItemCommand = new RelayCommand(CreateToDoItem);
    }

    private void CreateToDoItem()
    {
        var plainText = RtfConverter.ConvertToPlainText(TextData);
        var rawData = StringConverter.GetBytes(TextData);
        this.ToDoItem = new ToDoItem(plainText, rawData, new ItemCreationContext("ToDoLite"));
        CloseWindow?.Invoke();
    }

    public ICommand AddNewItemCommand { get; private set; }

    public ToDoItem? ToDoItem { get; set; }

    public string? TextData { get; set; }
    public Action? CloseWindow { get; set; }
}