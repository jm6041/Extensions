using System;
using System.Collections.Generic;
using System.Text;

namespace Jimlicat.FileHash
{
    /// <summary>
    /// <see cref="FileHashInfo"/>事件参数
    /// </summary>
    public class FileHashInfoEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileHashInfo"></param>
        public FileHashInfoEventArgs(FileHashInfo fileHashInfo)
        {
            FileHash = fileHashInfo;
        }

        /// <summary>
        /// 文件Hash
        /// </summary>
        public FileHashInfo FileHash { get; }
    }
}
