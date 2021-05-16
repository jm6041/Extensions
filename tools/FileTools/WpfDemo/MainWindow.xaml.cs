using MahApps.Metro.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LaunchGitHubSite(object sender, RoutedEventArgs e)
        {
            // Launch the GitHub site...
        }

        private void DeployCupCakes(object sender, RoutedEventArgs e)
        {
            // deploy some CupCakes...
        }
        private int row = 1;
        private int col = 1;
        private void SetBtn_Click(object sender, RoutedEventArgs e)
        {
            int mr = (int)Row.Value;
            int mc = (int)Col.Value;
            if (mr != row)
            {
                MyGrid.RowDefinitions.Clear();
                for (int i = 0; i < mr; i++)
                {
                    MyGrid.RowDefinitions.Add(new RowDefinition());
                }
                row = mr;
            }
            if (mc != col)
            {
                MyGrid.ColumnDefinitions.Clear();
                for (int i = 0; i < mc; i++)
                {
                    MyGrid.ColumnDefinitions.Add(new ColumnDefinition());
                }
                col = mc;
            }

            MyGrid.Children.Clear();
            Image[,] images = new Image[row, col];

            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    Image img = new Image();
                    BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/WpfDemo;component/images/t.jpg"));
                    img.Source = bitmapImage;
                    img.SetValue(Grid.RowProperty, r);
                    img.SetValue(Grid.ColumnProperty, c);
                    images[r, c] = img;
                    MyGrid.Children.Add(img);
                }
            }
        }
    }
}
