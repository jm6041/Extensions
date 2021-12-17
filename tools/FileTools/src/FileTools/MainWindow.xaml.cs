using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
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
using WinForm = System.Windows.Forms;
using Jimlicat.FileHash;
using System.Windows.Media.Animation;

namespace FileTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal WinForm.FolderBrowserDialog folderBrowserDialog;
        private readonly FilesView filesView;
        private readonly string DefaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InitializeOther();

            filesView = new FilesView() { FolderName = DefaultFolder };
            DataContext = filesView;
        }

        private void InitializeOther()
        {
            folderBrowserDialog = new WinForm.FolderBrowserDialog()
            {
                SelectedPath = DefaultFolder,
            };
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            filesView.FileHashComputed = false;
            WinForm.DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == WinForm.DialogResult.OK)
            {
                filesView.FolderName = folderBrowserDialog.SelectedPath;
            }
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filesView.FolderName))
            {
                return;
            }
            if (!Directory.Exists(filesView.FolderName))
            {
                return;
            }
            try
            {
                filesView.FileHashComputed = false;
                DirectoryInfo dirInfo = new DirectoryInfo(filesView.FolderName);
                FileInfo[] files = dirInfo.GetFiles("", SearchOption.AllDirectories);
                if (!files.Any())
                {
                    return;
                }
                filesView.Total = files.Length;
                filesView.FileItems.Clear();
                List<FileItemView> fis = new List<FileItemView>();
                foreach (var f in files)
                {
                    string rn = System.IO.Path.GetRelativePath(filesView.FolderName, f.FullName);
                    FileItemView fi = new FileItemView()
                    {
                        FileInfo = f,
                        Name = rn,
                        FullName = f.FullName,
                        Length = f.Length,
                        LastWriteTime = f.LastWriteTime,
                        CreationTime = f.CreationTime,
                    };
                    fis.Add(fi);
                    filesView.FileItems.Add(fi);
                }
                List<FileInfo> hashFiles = new List<FileInfo>();
                var lenGroup = fis.GroupBy(x => x.Length);
                foreach (var g in lenGroup)
                {
                    if (g.Count() >= 2)
                    {
                        hashFiles.AddRange(g.Select(x => x.FileInfo));
                    }
                }
                int hashCount = hashFiles.Count;
                filesView.Count = files.Length - hashCount;
                if (hashCount >= 1)
                {
                    FilesHashComputer computer = new FilesHashComputer(hashFiles);
                    computer.FileHashComputed += Computer_FileHashComputed;
                    await computer.ComputeHash();
                    filesView.FileHashComputed = true;
                }
            }
            catch (Exception ex)
            {
                PrintError(ex);
            }
        }

        private void Computer_FileHashComputed(object sender, FileHashInfoEventArgs e)
        {
            var fInfo = e.FileHash.FileInfo;
            string rn = System.IO.Path.GetRelativePath(filesView.FolderName, fInfo.FullName);
            Application.Current.Dispatcher.Invoke(() =>
            {
                var f = filesView.FileItems.FirstOrDefault(x => x.FileInfo == fInfo);
                if (f != null)
                {
                    f.SHA256 = FilesHashComputer.ToHexString(e.FileHash.SHA256);
                }
                int count = filesView.Count + 1;
                filesView.Count = count;
            });
        }

        private void PrintError(Exception ex)
        {
            string msg = ex.GetType().Name + ": " + ex.Message;
            ShowErrorMsg(msg, false);
        }

        private void BtnRemoveDup_Click(object sender, RoutedEventArgs e)
        {
            List<FileItemView> removeFiles = new List<FileItemView>();
            foreach (var files in filesView.FileItems.Where(x => false == string.IsNullOrEmpty(x.SHA256)).GroupBy(x => x.SHA256))
            {
                removeFiles.AddRange(files.OrderBy(x => x.CreationTime).Skip(1));
            }
            if (removeFiles.Any())
            {
                try
                {
                    DirectoryInfo rdir = GetRemoveDir();
                    foreach (var rf in removeFiles)
                    {
                        if (File.Exists(rf.FullName))
                        {
                            string newFile = System.IO.Path.Combine(rdir.FullName, rf.Name);
                            string newDir = System.IO.Path.GetDirectoryName(newFile);
                            Directory.CreateDirectory(newDir);
                            File.Move(rf.FullName, newFile, false);
                            filesView.FileItems.Remove(rf);
                        }
                    };
                    int count = filesView.FileItems.Count;
                    filesView.Total = count;
                    filesView.Count = count;
                }
                catch (Exception ex)
                {
                    PrintError(ex);
                }
            }
        }

        /// <summary>
        ///  获得移除文件夹
        /// </summary>
        /// <returns></returns>
        private DirectoryInfo GetRemoveDir()
        {
            DirectoryInfo dir = new DirectoryInfo(filesView.FolderName);
            string rdn = dir.Name + "_dup_" + Guid.NewGuid().ToString("N");
            string path = System.IO.Path.Combine(dir.Parent.FullName, rdn);
            if (!Directory.Exists(path))
            {
                return Directory.CreateDirectory(path);
            }
            return new DirectoryInfo(path);
        }
        private async Task ShowFileDetailsDialog(FileItemView fv)
        {
            if (string.IsNullOrEmpty(fv.SHA256) || string.IsNullOrEmpty(fv.PreBytes))
            {
                FileStream fs = fv.FileInfo.Open(FileMode.Open);
                if (string.IsNullOrEmpty(fv.PreBytes))
                {
                    fs.Position = 0;
                    byte[] prebs = new byte[4];
                    await fs.ReadAsync(prebs, 0, 4);
                    fv.PreBytes = FilesHashComputer.ToHexString(prebs);
                }
                if (string.IsNullOrEmpty(fv.SHA256))
                {
                    fs.Position = 0;
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        fv.SHA256 = "Sha256正在计算中...";
                        byte[] hv = await sha256.ComputeHashAsync(fs);
                        fv.SHA256 = FilesHashComputer.ToHexString(hv);
                    }
                }
                fs.Close();
            }
            var fdd = new FileDetailsDialog(fv);
            fdd.ShowDialog();
        }
        private async void FileDetails_Click(object sender, RoutedEventArgs e)
        {
            if (fileGrid.SelectedValue is FileItemView fv)
            {
                await ShowFileDetailsDialog(fv);
            }
        }
        private async void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (fileGrid.SelectedValue is FileItemView fv)
            {
                await ShowFileDetailsDialog(fv);
            }
        }
        // 红色
        private static readonly SolidColorBrush RedColorBrush = new SolidColorBrush(Colors.Red);
        // 绿色
        private static readonly SolidColorBrush GreenColorBrush = new SolidColorBrush(Colors.Green);
        /// <summary>
        /// 显示成功消息
        /// </summary>
        /// <param name="msg"></param>
        private void ShowSuccessMsg(string msg)
        {
            textMsg.Opacity = 1;
            textMsg.Foreground = GreenColorBrush;
            textMsg.Text = msg;
            if (FindResource("hideMsg") is Storyboard storyboard)
            {
                storyboard.Begin(textMsg);
            }
        }
        /// <summary>
        /// 显示异常消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="showBox"></param>
        private void ShowErrorMsg(string msg, bool showBox = true)
        {
            textMsg.Opacity = 1;
            textMsg.Foreground = RedColorBrush;
            textMsg.Text = msg;
            if (FindResource("hideMsg") is Storyboard storyboard)
            {
                storyboard.Begin(textMsg, true);
            }
            if (showBox)
            {
                MessageBox.Show(msg, "异常", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
