using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Eson.Extensions.Logging.File
{
    /// <summary>
    /// 文件日志提供者
    /// </summary>
    [ProviderAlias("File")]
    public class FileLoggerProvider : BatchingLoggerProvider
    {
        private readonly string _path;
        private readonly string _fileName;
        // 文件全路径名
        private string _fullFileName;
        private readonly int? _maxFileSize;
        private readonly int? _maxRetainedFiles;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"><see cref="FileLoggerOptions"/></param>
        public FileLoggerProvider(IOptionsMonitor<FileLoggerOptions> options) : base(options)
        {
            var loggerOptions = options.CurrentValue;
            _path = loggerOptions.LogDirectory;
            // 确定目录存在
            Directory.CreateDirectory(_path);
            _fileName = loggerOptions.FileName;

            _maxFileSize = loggerOptions.FileSizeLimit;
            _maxRetainedFiles = loggerOptions.RetainedFileCountLimit;
        }

        /// <summary>
        /// 获得文件全路径名
        /// </summary>
        private string GetFullFileName((int Year, int Month, int Day) group)
        {
            return LazyInitializer.EnsureInitialized(ref _fullFileName,
                    () => { return CombineName(group); });
        }
        /// <summary>
        /// 写日志核心方法
        /// </summary>
        /// <param name="messages">消息</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        protected override async Task WriteMessagesAsync(IEnumerable<LogMessage> messages, CancellationToken cancellationToken)
        {
            foreach (var group in messages.GroupBy(msg => { return (msg.Timestamp.Year, msg.Timestamp.Month, msg.Timestamp.Day); }))
            {
                string fn = GetFullFileName(group.Key);
                var fileInfo = new FileInfo(fn);
                if (_maxFileSize > 0 && fileInfo.Exists && fileInfo.Length > _maxFileSize)
                {
                    _fullFileName = null;
                    fn = GetFullFileName(group.Key);
                    return;
                }

                using (var streamWriter = System.IO.File.AppendText(fn))
                {
                    foreach (var item in group)
                    {
                        await streamWriter.WriteAsync(item.Message);
                    }
                }
            }

            RollFiles();
        }
        // 连接文件路径名
        private string CombineName((int Year, int Month, int Day) group)
        {
            return Path.Combine(_path, $"{_fileName}{group.Year:0000}{group.Month:00}{group.Day:00}-{DateTime.Now.Ticks}.log");
        }
        /// <summary>
        /// 确定删除文件
        /// </summary>
        protected void RollFiles()
        {
            if (_maxRetainedFiles > 0)
            {
                var files = new DirectoryInfo(_path)
                    .GetFiles(_fileName + "*")
                    .OrderByDescending(f => f.Name)
                    .Skip(_maxRetainedFiles.Value);

                foreach (var item in files)
                {
                    item.Delete();
                }
            }
        }
    }
}
