using System.Windows;
using System.Windows.Controls;

namespace Jimlicat.Windows.Controls
{
    /// <summary>
    /// 自定义时间输入控件
    /// </summary>
    public class KibaDateTime : TextBox
    {
        /// <summary>
        /// 小时
        /// </summary>
        public static readonly DependencyProperty HourProperty = DependencyProperty.Register(
             "Hour", typeof(int), typeof(KibaDateTime), new FrameworkPropertyMetadata(00));
        /// <summary>
        /// 小时
        /// </summary>
        public int Hour
        {
            get
            {
                return (int)GetValue(HourProperty);
            }
            set
            {
                SetValue(HourProperty, value);
            }
        }
        /// <summary>
        /// 分钟
        /// </summary>
        public static readonly DependencyProperty MinuteProperty = DependencyProperty.Register(
             "Minute", typeof(int), typeof(KibaDateTime), new FrameworkPropertyMetadata(00));
        /// <summary>
        /// 分钟
        /// </summary>
        public int Minute
        {
            get
            {
                return (int)GetValue(MinuteProperty);
            }
            set
            {
                SetValue(MinuteProperty, value);
            }
        }
        /// <summary>
        /// 秒
        /// </summary>
        public static readonly DependencyProperty SecondProperty = DependencyProperty.Register(
             "Second", typeof(int), typeof(KibaDateTime), new FrameworkPropertyMetadata(00));
        /// <summary>
        /// 秒
        /// </summary>
        public int Second
        {
            get
            {
                return (int)GetValue(SecondProperty);
            }
            set
            {
                SetValue(SecondProperty, value);
            }
        }
        static KibaDateTime()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KibaDateTime), new FrameworkPropertyMetadata(typeof(KibaDateTime)));
        }
    }
}
