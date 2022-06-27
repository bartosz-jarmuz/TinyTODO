using System.ComponentModel;

namespace ToDoLite.Core.Persistence
{
    public partial class Settings : ISettings
    {
        private Settings()
        {
            var dbContext = new ToDoLiteDbContext();
            _dataAccess = new DataAccess(dbContext);
            
        }

        private readonly DataAccess _dataAccess;
        private static readonly Lazy<ISettings> _lazy = new(() => new Settings());
        public static ISettings Instance => _lazy.Value;

        [Description("Set to true to see completed tasks on the list.")]
        public bool ShowCompleted { get => _dataAccess.Get<bool>(Keys.ShowCompleted, false); set => _dataAccess.Set(Keys.ShowCompleted, value); }
        
        [Description("When true, the app will be hidden to System Tray when Close button is clicked.")]
        public bool CloseToTray { get => _dataAccess.Get<bool>(Keys.CloseToTray, false); set => _dataAccess.Set(Keys.CloseToTray, value); }

        [Description("When true, the app will be hidden to System Tray when Minimize button is clicked.")]
        public bool MinimizeToTray { get => _dataAccess.Get<bool>(Keys.MinimizeToTray, true); set => _dataAccess.Set(Keys.MinimizeToTray, value); }

        private static class Keys
        {
            public const string ShowCompleted = "ShowCompleted";
            public const string CloseToTray = "CloseToTray";
            public const string MinimizeToTray = "MinimizeToTray";
        }
    }
}
