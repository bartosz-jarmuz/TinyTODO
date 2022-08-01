using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToDoLite.App.Windows
{
    /// <summary>
    /// Interaction logic for FullSizePreviewWindow.xaml
    /// </summary>
    public partial class FullSizePreviewWindow : Window
    {
        public FullSizePreviewWindow()
        {
            InitializeComponent();
        }

        private void Image_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
        }
    }
}
