using System.Windows.Controls;
using System.Windows.Input;

namespace ToDoLite.App.Windows.Controls;

public class ToDoItemsListBox : ListBox
{
    protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
    {

        if (e.OriginalSource is Image { Parent: ZoomBorder border })
        {
            border.HandleWheelEvent(this, e);
            e.Handled = true;
            return;
        }
        //if (Keyboard.Modifiers == ModifierKeys.Control)
        //{
        //}
        base.OnPreviewMouseWheel(e);
    }
}