using ItemsSummary.Common.Services;
using ItemsSummary.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ItemsSummary.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var serviceProvider = ConfigServiceProvider();
            //MainWindowのDataContextを設定し表示する
            MainWindow mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindow.Show();
        }

        /// <summary>
        /// DI Containerの設定
        /// </summary>
        /// <returns></returns>
        private IServiceProvider ConfigServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IEnvironmentService, EnvironmentService>();
            //services.AddSingleton<IFileService, FileService>();
            //Demo Serviceを使用
            services.AddSingleton<IFileService, FileServiceForDemo>();

            services.AddSingleton<MainWindowViewModel>();
            //services.AddTransient<PoFileInfoViewModel>();
            services.AddSingleton<MainWindow>();
            return services.BuildServiceProvider();
        }
    }

}
