using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoLite.App.Windows.TextToColor;
using ToDoLite.Core;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;

namespace ToDoLite.App.Windows.ViewModel
{
    public class TagViewModel : ObservableObject
    {
        public TagViewModel(Tag tag)
        {
            Tag = tag;
            Name = tag.Name;
            Description = tag.Description;
            BackColor = TextToColorConverter.Instance.GetBrush(tag.Name);
        }

        public Tag Tag { get; }
        public string Name { get; }
        public string? Description { get; }
        public Brush? BackColor { get; }
    }
}
