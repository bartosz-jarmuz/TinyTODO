using System.ComponentModel;

namespace TinyTODO.App.Windows;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public void SetPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}