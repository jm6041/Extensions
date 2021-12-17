using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FileTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
            {
                MessageBox.Show(error.ExceptionObject.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
