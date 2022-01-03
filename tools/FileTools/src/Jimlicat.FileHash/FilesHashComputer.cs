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
    /// 文件hash计算异常事件处理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void FileHashErrorEventHandler(object sender, FileHashErrorEventArgs e);

    /// <summary>
    /// 计算文件Hash
    /// </summary>
    public class FilesHashComputer
    {
        private readonly ICollection<FileInfo> _files;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="files">文件信息</param>
        public FilesHashComputer(ICollection<FileInfo> files)
        {
            _files = files;
        }
        /// <summary>
        /// 文件hash计算完成事件
        /// </summary>
        public event FileHashComputedEventHandler FileHashComputed;
        /// <summary>
        /// 文件hash计算完成事件
        /// </summary>
        public event FileHashErrorEventHandler FileHashError;
        /// <summary>
        /// 计算Hash
        /// </summary>
        public Task<FileHashInfo[]> ComputeHash()
        {
            List<Task<FileHashInfo>> tasks = new List<Task<FileHashInfo>>();
            foreach (var file in _files)
            {
                Task<FileHashInfo> task = ComputeFileHash(file);
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
        private async Task<FileHashInfo> ComputeFileHash(FileInfo file)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                try
                {
                    using FileStream fileStream = file.Open(FileMode.Open);
                    fileStream.Position = 0;
                    byte[] hv = await sha256.ComputeHashAsync(fileStream);
                    FileHashInfo fi = new FileHashInfo(file, hv);
                    fileStream.Close();
                    FileHashComputed?.Invoke(this, new FileHashInfoEventArgs(fi));
                    return fi;
                }
                catch (Exception ex)
                {
                    FileHashError?.Invoke(this, new FileHashErrorEventArgs(new FileHashError(file, ex)));
                    return new FileHashInfo(file, null);
                }
            }
        }
        /// <summary>
        /// byte数组转换为字符串
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] array)
        {
            return Convert.ToHexString(array);
        }
    }
}
