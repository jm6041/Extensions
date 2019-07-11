using System;

namespace Eson.Extensions.Logging.File
{
    /// <summary>
    /// 日志文件设置选项
    /// </summary>
    public class FileLoggerOptions : BatchingLoggerOptions
    {
        private int? _fileSizeLimit = 20 * 1024 * 1024;
        private int? _retainedFileCountLimit = 40;
        private string _fileName = "logs-";


        /// <summary>
        /// 日志文件占用空间限制，如果为空，表示不限制大小，默认为 <c>20MB</c>
        /// </summary>
        public int? FileSizeLimit
        {
            get { return _fileSizeLimit; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(FileSizeLimit)} must be positive.");
                }
                _fileSizeLimit = value;
            }
        }

        /// <summary>
        /// 最大日志文件数量，如果为空，表示不限制文件数量，默认为 <c>40</c>
        /// </summary>
        public int? RetainedFileCountLimit
        {
            get { return _retainedFileCountLimit; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(RetainedFileCountLimit)} must be positive.");
                }
                _retainedFileCountLimit = value;
            }
        }

        /// <summary>
        /// 文件名前缀，默认为 <c>logs-</c>
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(nameof(value));
                }
                _fileName = value;
            }
        }

        /// <summary>
        /// 日志文件的目录，默认为 <c>logs</c>
        /// </summary>
        public string LogDirectory { get; set; } = "logs";
    }
}
