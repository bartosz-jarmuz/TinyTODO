using System.Windows;
using TinyTODO.Core.Contracts;
using TinyTODO.Core.DataModel;

namespace TinyTODO.Core.Windows;

public class Presenter : IToDoItemPresenter
{
    public Presenter()
    {
    }

    public void Present(ToDoItem item)
    {
        Clipboard.SetText(item.ToString());
        MessageBox.Show(item.Data.ToString(), item.Context.ActiveWindowTitle);
    }
}
