using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            logger = AppServices.Services.GetRequiredService<ILogger<MainWindow>>();
        }

        private void BtnLogDebug_Click(object sender, RoutedEventArgs e)
        {
            logger.LogDebug("这是 LogDebug");
        }

        private void BtnLogInfo_Click(object sender, RoutedEventArgs e)
        {
            logger.LogInformation("这是 LogInformation");
        }

        private void BtnLogWarning_Click(object sender, RoutedEventArgs e)
        {
            logger.LogWarning("这是 LogWarning");
        }
    }
}
