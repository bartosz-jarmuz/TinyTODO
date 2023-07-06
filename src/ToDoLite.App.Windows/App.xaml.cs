using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ToDoLite.App.Windows.ViewModel;
using ToDoLite.Core;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.Persistence;
using ToDoLite.Core.Windows;

namespace ToDoLite.App.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();

            this.InitializeComponent();
        }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IConfirmationEmitter, ConsoleBeepEmitter>();
            services.AddSingleton<IClipboardDataProvider, WindowsClipboardDataProvider>();
            services.AddSingleton<IContextProvider, WindowsContextProvider>();
            services.AddSingleton<IToDoItemGenerator, ToDoItemGenerator>();

            var storage = new SqliteToDoItemStorage();
            services.AddSingleton<IToDoItemStorage>(storage);
            services.AddSingleton<ITagRepository>(storage);

            services.AddSingleton<IDataExporter, JsonDataExporter>();
            services.AddTransient<MainWindowViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
