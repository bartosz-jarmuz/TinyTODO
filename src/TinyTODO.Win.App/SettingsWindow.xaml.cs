using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TinyTODO.Core;

namespace TinyTODO.App.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        public SettingsWindow(ISettings settings)
        {
            _settings = settings;
            DataContext = this;
            InitializeComponent();
        }


        private ISettings _settings;

        public ISettings Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
