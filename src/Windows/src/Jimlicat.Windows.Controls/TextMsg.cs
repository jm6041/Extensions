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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jimlicat.Windows.Controls
{
    /// <summary>
    /// 消息文本
    /// </summary>
    public class TextMsg : TextBox
    {
        static TextMsg()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextMsg), new FrameworkPropertyMetadata(typeof(TextMsg)));
        }
        /// <summary>
        /// 红色画刷
        /// </summary>
        public static readonly SolidColorBrush RedColorBrush = new SolidColorBrush(Colors.Red);
        /// <summary>
        /// 绿色画刷
        /// </summary>
        public static readonly SolidColorBrush GreenColorBrush = new SolidColorBrush(Colors.Green);
        ///// <summary>
        ///// 显示消息
        ///// </summary>
        ///// <param name="msg"></param>
        //public void ShowMsg(string msg)
        //{
        //    if (FindResource("hideMsg") is Storyboard storyboard)
        //    {
        //        storyboard.Remove(textMsg);
        //    }
        //    textMsg.Opacity = 1;
        //    textMsg.Foreground = GreenColorBrush;
        //    textMsg.Text = msg;
        //}
        ///// <summary>
        ///// 显示消息
        ///// </summary>
        ///// <param name="msg"></param>
        ///// <param name="msgs"></param>
        //public void ShowMsgs(string msg, IEnumerable<string>? msgs = null)
        //{
        //    if (msgs == null)
        //    {
        //        ShowMsg(msg);
        //    }
        //    else
        //    {
        //        List<string> list = new List<string>();
        //        if (!string.IsNullOrEmpty(msg))
        //        {
        //            list.Add(msg);
        //        }
        //        foreach (string m in msgs)
        //        {
        //            if (!string.IsNullOrEmpty(m))
        //            {
        //                list.Add(m);
        //            }
        //        }
        //        var text = string.Join(";", list);
        //        ShowMsg(text);
        //    }
        //}
        ///// <summary>
        ///// 显示成功消息
        ///// </summary>
        ///// <param name="msg"></param>
        //public void ShowSuccessMsg(string msg)
        //{
        //    textMsg.Opacity = 1;
        //    textMsg.Foreground = GreenColorBrush;
        //    textMsg.Text = msg;
        //    if (textMsg.FindResource("hideMsg") is Storyboard storyboard)
        //    {
        //        storyboard.Begin(textMsg, true);
        //    }
        //}
        ///// <summary>
        ///// 显示成功消息
        ///// </summary>
        ///// <param name="msg"></param>
        ///// <param name="msgs"></param>
        //public void ShowSuccessMsgs(string msg, IEnumerable<string>? msgs = null)
        //{
        //    if (msgs == null)
        //    {
        //        ShowSuccessMsg(msg);
        //    }
        //    else
        //    {
        //        List<string> list = new List<string>();
        //        if (!string.IsNullOrEmpty(msg))
        //        {
        //            list.Add(msg);
        //        }
        //        foreach (string m in msgs)
        //        {
        //            if (!string.IsNullOrEmpty(m))
        //            {
        //                list.Add(m);
        //            }
        //        }
        //        var text = string.Join(";", list);
        //        ShowSuccessMsg(text);
        //    }
        //}
        ///// <summary>
        ///// 显示异常消息
        ///// </summary>
        ///// <param name="msg"></param>
        ///// <param name="autoHiden"></param>
        ///// <param name="ex"></param>
        //public string ShowErrorMsg(string msg, bool autoHiden = true, Exception? ex = null)
        //{
        //    textMsg.Opacity = 1;
        //    textMsg.Foreground = RedColorBrush;
        //    msg = GetErrorMsg(msg, ex);
        //    textMsg.Text = msg;
        //    if (textMsg.FindResource("hideMsg") is Storyboard storyboard)
        //    {
        //        if (autoHiden)
        //        {
        //            storyboard.Begin(textMsg, true);
        //        }
        //        else
        //        {
        //            storyboard.Remove(textMsg);
        //        }
        //    }
        //    return msg;
        //}
        ///// <summary>
        ///// 获得异常消息
        ///// </summary>
        ///// <param name="msg"></param>
        ///// <param name="ex"></param>
        ///// <returns></returns>
        //protected virtual string GetErrorMsg(string msg, Exception? ex)
        //{
        //    if (ex != null)
        //    {
        //        StringBuilder b = new StringBuilder(msg);
        //        b.Append(" Error:").Append(ex.Message);
        //        msg = b.ToString();
        //    }
        //    return msg;
        //}
        ///// <summary>
        ///// 显示异常消息
        ///// </summary>
        ///// <param name="autoHiden"></param>
        ///// <param name="msgs"></param>
        //public string ShowErrorMsgs(IEnumerable<string> msgs, bool autoHiden = true)
        //{
        //    textMsg.Opacity = 1;
        //    textMsg.Foreground = RedColorBrush;
        //    var msg = string.Join(";", msgs);
        //    textMsg.Text = msg;
        //    if (textMsg.FindResource("hideMsg") is Storyboard storyboard)
        //    {
        //        if (autoHiden)
        //        {
        //            storyboard.Begin(textMsg, true);
        //        }
        //        else
        //        {
        //            storyboard.Remove(textMsg);
        //        }
        //    }
        //    return msg;
        //}
        ///// <summary>
        ///// 显示Excel导出异常
        ///// </summary>
        ///// <param name="ex"></param>
        //public string ShowExcelExpErrorMsg(Exception? ex = null)
        //{
        //    var m = CRC.ExcelExpError;
        //    view.FileFullPath = null;
        //    ShowErrorMsg(m, true, ex);
        //    return m;
        //}
        ///// <summary>
        ///// 显示Excel导出成功
        ///// </summary>
        ///// <param name="fn">文件名</param>
        //public string ShowExcelExpSuccessMsg(string? fn = null)
        //{
        //    var m = CRC.ExcelExpSuccess;
        //    if (fn != null)
        //    {
        //        m += " File: " + fn;
        //        if (File.Exists(fn))
        //        {
        //            view.FileFullPath = fn;
        //        }
        //        ShowMsg(m);
        //        return m;
        //    }
        //    else
        //    {
        //        ShowSuccessMsg(m);
        //        return m;
        //    }
        //}
        //private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        //{
        //    if (view.FileFullPath != null && view.FileFullPath.Length != 0)
        //    {
        //        ExplorerHelper.OpenInExplorer(view.FileFullPath);
        //    }
        //}
    }
}
