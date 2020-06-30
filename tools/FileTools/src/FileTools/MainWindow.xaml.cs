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
                filesView.Total = files.Length;
                filesView.FileItems.Clear();
                filesView.Count = 0;
                FilesHashComputer computer = new FilesHashComputer(files);
                computer.FileHashComputed += Computer_FileHashComputed;
                await computer.ComputeHash();
                filesView.FileHashComputed = true;
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
                FileItemView fi = new FileItemView()
                {
                    Name = rn,
                    FullName = fInfo.FullName,
                    Length = fInfo.Length,
                    LastWriteTime = fInfo.LastWriteTime,
                    CreationTime = fInfo.CreationTime,
                    SHA256 = FilesHashComputer.ByteArrayToString(e.FileHash.SHA256),
                };
                filesView.FileItems.Add(fi);
                int count = filesView.Count + 1;
                filesView.Count = count;
            });
        }

        private void PrintError(Exception ex)
        {
            TextError.Text = ex.GetType().Name + ": " + ex.Message;
        }

        private void BtnRemoveDup_Click(object sender, RoutedEventArgs e)
        {
            List<FileItemView> removeFiles = new List<FileItemView>();
            foreach (var files in filesView.FileItems.GroupBy(x => x.SHA256))
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
            string rdn = dir.Name + "_dup_"+Guid.NewGuid().ToString("N");            
            string path = System.IO.Path.Combine(dir.Parent.FullName, rdn);
            if (!Directory.Exists(path))
            {
                return Directory.CreateDirectory(path);
            }
            return new DirectoryInfo(path);
        }
    }
}
