using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace FileTools
{
    /// <summary>
    /// 文件
    /// </summary>
    public class FilesView : INotifyPropertyChanged
    {
        private string folderName;
        /// <summary>
        /// 文件夹名
        /// </summary>
        public string FolderName
        {
            get => folderName;
            set
            {
                string v = value;
                if (v != null)
                {
                    v = v.Trim();
                }
                SetProperty(ref folderName, v);
            }
        }

        private int total;
        /// <summary>
        /// 总数
        /// </summary>
        public int Total
        {
            get => total;
            set => SetProperty(ref total, value);
        }

        private int count;
        /// <summary>
        /// 处理数量
        /// </summary>
        public int Count
        {
            get => count;
            set => SetProperty(ref count, value);
        }

        private bool fileHashComputed;
        /// <summary>
        /// 文件hash计算是否完成
        /// </summary>
        public bool FileHashComputed
        {
            get => fileHashComputed;
            set => SetProperty(ref fileHashComputed, value);
        }
        /// <summary>
        /// 文件项集合
        /// </summary>
        public ObservableCollection<FileItemView> FileItems { get; set; } = new ObservableCollection<FileItemView>();
        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T field, T newValue, string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 文件项
    /// </summary>
    public class FileItemView
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 全民
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// 大小描述
        /// </summary>
        public string LengthDesc
        {
            get
            {
                if (Length < 1024)
                {
                    return Length + " B";
                }
                else if (Length < 1024 * 1024)
                {
                    return Math.Round(Length / 1024d, 2).ToString("f2") + " KB";
                }
                else if (Length < 1024 * 1024 * 1024)
                {
                    return Math.Round(Length / 1024d / 1024d, 2).ToString("f2") + " MB";
                }
                else
                {
                    return Math.Round(Length / 1024d / 1024d / 1024d, 2).ToString("f2") + " GB";
                }
            }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime LastWriteTime { get; set; }
        /// <summary>
        /// 修改时间描述
        /// </summary>
        public string LastWriteTimeDesc
        {
            get => LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// SHA256
        /// </summary>
        public string SHA256 { get; set; }
    }
}
