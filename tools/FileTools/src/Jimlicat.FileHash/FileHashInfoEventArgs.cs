using System;
using System.Collections.Generic;
using System.Text;

namespace Jimlicat.FileHash
{
    /// <summary>
    /// <see cref="FileHashInfo"/>事件参数
    /// </summary>
    public class FileHashErrorEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileHashError"></param>
        public FileHashErrorEventArgs(FileHashError fileHashError)
        {
            FileHashError = fileHashError;
        }

        /// <summary>
        /// 文件Hash异常
        /// </summary>
        public FileHashError FileHashError { get; }
    }
}
