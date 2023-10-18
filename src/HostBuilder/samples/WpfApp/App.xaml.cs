using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly MainWindow mainWindow = null!;
        private readonly ILogger logger = null!;
        /// <summary>
        /// 构造函数
        /// </summary>
        public App()
        {
            try
            {
                ServiceProvider = AppServices.Init();
                mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                logger = ServiceProvider.GetRequiredService<ILogger<App>>();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "初始化应用异常", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
        }
        /// <summary>
        /// IServiceProvider
        /// </summary>
        public IServiceProvider ServiceProvider { get; } = null!;
        private void OnStartup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            try
            {
                MainWindow = mainWindow;
                MainWindow.Show();
            }
            catch (Exception ex)
            {
                const string msg = "启动应用出错";
                logger.LogError(ex, msg);
                MessageBox.Show(ex.ToString(), msg, MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var msg = e.ExceptionObject.ToString();
            logger.LogError(msg);

#if DEBUG
#else
                msg = "有未处理异常，详情查看日志！";
#endif
            MessageBox.Show(msg, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            Current.Shutdown(e.ApplicationExitCode);
        }
    }
}
