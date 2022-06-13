using Microsoft.EntityFrameworkCore;
using TinyTODO.Core.Contracts;

namespace TinyTODO.Core
{
    public partial class Settings : ISettings
    {
        private Settings()
        {
            _dbContext = new TinyToDoDbContext();
            _dataAccess = new DataAccess(_dbContext);
            
        }

        private readonly TinyToDoDbContext _dbContext;
        private bool disposedValue;
        private DataAccess _dataAccess;
        private static readonly Lazy<ISettings> lazy = new Lazy<ISettings>(() => new Settings());
        public static ISettings Instance { get { return lazy.Value; } }

        public bool ShowCompleted { get => _dataAccess.Get<bool>(Keys.ShowCompleted, false); set => _dataAccess.Set(Keys.ShowCompleted, value); }
        public bool CloseToTray { get => _dataAccess.Get<bool>(Keys.CloseToTray, false); set => _dataAccess.Set(Keys.CloseToTray, value); }
        public bool MinimizeToTray { get => _dataAccess.Get<bool>(Keys.MinimizeToTray, true); set => _dataAccess.Set(Keys.MinimizeToTray, value); }

        private static class Keys
        {
            public const string ShowCompleted = "ShowCompleted";
            public const string CloseToTray = "CloseToTray";
            public const string MinimizeToTray = "MinimizeToTray";
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
