using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Jimlicat.FileHash
{
    /// <summary>
    /// 文件hash计算完毕事件处理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void FileHashComputedEventHandler(object sender, FileHashInfoEventArgs e);

    /// <summary>
    /// 计算文件Hash
    /// </summary>
    public class FilesHashComputer
    {
        private readonly FileInfo[] _files;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="files">文件信息</param>
        public FilesHashComputer(FileInfo[] files)
        {
            _files = files;
        }
        /// <summary>
        /// 文件hash计算完成事件
        /// </summary>
        public event FileHashComputedEventHandler FileHashComputed;
        /// <summary>
        /// 计算Hash
        /// </summary>
        public Task<FileHashInfo[]> ComputeHash()
        {
            List<Task<FileHashInfo>> tasks = new List<Task<FileHashInfo>>();
            foreach (var file in _files)
            {
                Task<FileHashInfo> task = Task.Factory.StartNew(() =>
                {
                    return ComputeFileHash(file);
                });
                tasks.Add(task);
            }
            if (tasks.Any())
            {
                return Task.Factory.ContinueWhenAll(tasks.ToArray(), (completedTasks) =>
                {
                    return completedTasks.Where(x => !x.IsFaulted).Select(x => x.Result).ToArray();
                });
            }
            return Task.FromResult(Array.Empty<FileHashInfo>());
        }
        /// <summary>
        /// 计算文件Hash
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public FileHashInfo ComputeFileHash(FileInfo file)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                FileStream fileStream = file.Open(FileMode.Open);
                fileStream.Position = 0;
                byte[] hv = sha256.ComputeHash(fileStream);
                FileHashInfo fi = new FileHashInfo(file, hv);
                fileStream.Close();
                FileHashComputed?.Invoke(this, new FileHashInfoEventArgs(fi));
                return fi;
            }
        }
        /// <summary>
        /// byte数组转换为字符串
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] array)
        {
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                b.AppendFormat($"{array[i]:X2}");
            }
            return b.ToString();
        }
    }
}
