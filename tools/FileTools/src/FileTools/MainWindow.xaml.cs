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

        public MainWindow()
        {
            InitializeComponent();
            InitializeOther();

            filesView = new FilesView() { FolderName = DefaultFolder };
            DataContext = filesView;
        }

        public void InitializeOther()
        {
            folderBrowserDialog = new WinForm.FolderBrowserDialog()
            {
                SelectedPath = DefaultFolder,
            };
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            WinForm.DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == WinForm.DialogResult.OK)
            {
                filesView.FolderName = folderBrowserDialog.SelectedPath;
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SetFileItems(filesView, filesView.FolderName);
        }

        private void SetFileItems(FilesView fv, string dir)
        {
            if (string.IsNullOrEmpty(dir))
            {
                return;
            }
            dir = dir.Trim();
            try
            {
                if (!Directory.Exists(dir))
                {
                    return;
                }
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                FileInfo[] files = dirInfo.GetFiles("", SearchOption.AllDirectories);
                fv.Total = files.Length;
                fv.FileItems.Clear();
                using (SHA256 sha256 = SHA256.Create())
                {
                    int num = 0;
                    foreach (FileInfo fInfo in files)
                    {
                        num++;
                        FileStream fileStream = fInfo.Open(FileMode.Open);
                        fileStream.Position = 0;
                        byte[] hv = sha256.ComputeHash(fileStream);
                        string hvs = ByteArrayToString(hv);
                        // 相对文件夹路径的文件名
                        string rn = System.IO.Path.GetRelativePath(dir, fInfo.FullName);
                        FileItemView fi = new FileItemView()
                        {
                            Name = rn,
                            FullName = fInfo.FullName,
                            Length = fInfo.Length,
                            LastWriteTime = fInfo.LastWriteTime,
                            CreationTime = fInfo.CreationTime,
                            SHA256 = hvs,
                        };
                        fv.FileItems.Add(fi);
                        fv.Count = num;
                        fileStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                PrintError(ex);
            }
        }
        public static string ByteArrayToString(byte[] array)
        {
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                b.AppendFormat($"{array[i]:X2}");
            }
            return b.ToString();
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
