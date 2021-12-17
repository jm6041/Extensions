using System;
using System.IO;

namespace Jimlicat.FileHash
{
    /// <summary>
    /// 文件Hash异常
    /// </summary>
    public class FileHashError
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ex"></param>
        public FileHashError(FileInfo file, Exception ex)
        {
            FileInfo = file;
            Error = ex;
        }

        /// <summary>
        /// 文件信息
        /// </summary>
        public FileInfo FileInfo { get; }
        /// <summary>
        /// 异常
        /// </summary>
        public Exception Error { get; }
    }
}
