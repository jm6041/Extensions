using System;
using System.IO;

namespace Jimlicat.FileHash
{
    /// <summary>
    /// 文件Hash信息
    /// </summary>
    public class FileHashInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="file"></param>
        /// <param name="sha256"></param>
        public FileHashInfo(FileInfo file, byte[] sha256)
        {
            FileInfo = file;
            SHA256 = sha256;
        }

        /// <summary>
        /// 文件信息
        /// </summary>
        public FileInfo FileInfo { get; }
        /// <summary>
        /// SHA256
        /// </summary>
        public byte[] SHA256 { get; }
    }
}
