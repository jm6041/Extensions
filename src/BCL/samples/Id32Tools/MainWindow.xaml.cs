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

namespace Id32Tools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(InputCount.Text?.Trim(), out int count))
            {
                count = 1;
            }
            if (count <= 0)
            {
                count = 1;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                var nid = Id32.NewId();
                sb.Append(nid).AppendLine();
            }
            OutResult.Clear();
            OutResult.AppendText(sb.ToString());
        }

        private void OutResult_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
