// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// 参考 Microsoft.Extensions.Logging.AzureAppServices.Internal.BatchingLoggerProvider 改写

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Eson.Extensions.Logging
{
    /// <summary>
    /// 批量日志提供者
    /// 参考 Microsoft.Extensions.Logging.AzureAppServices.Internal.BatchingLoggerProvider 改写
    /// </summary>
    public abstract class BatchingLoggerProvider : ILoggerProvider
    {
        private readonly List<LogMessage> _currentBatch = new List<LogMessage>();
        private readonly TimeSpan _interval;
        private readonly int? _queueSize;
        private readonly int? _batchSize;

        private readonly int _logMaxLength;
        private readonly ICollection<int> _eventIds;
        private readonly bool _includeException;

        private BlockingCollection<LogMessage> _messageQueue;
        private Task _outputTask;
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// ...
        /// </summary>
        public const string ContinueString = "...";
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"><see cref="BatchingLoggerOptions"/>配置</param>
        protected BatchingLoggerProvider(IOptionsMonitor<BatchingLoggerOptions> options)
        {
            // NOTE: Only IsEnabled is monitored

            var loggerOptions = options.CurrentValue;
            if (loggerOptions.BatchSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(loggerOptions.BatchSize), $"{nameof(loggerOptions.BatchSize)} must be a positive number.");
            }
            if (loggerOptions.FlushPeriod <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(loggerOptions.FlushPeriod), $"{nameof(loggerOptions.FlushPeriod)} must be longer than zero.");
            }

            _interval = loggerOptions.FlushPeriod;
            _batchSize = loggerOptions.BatchSize;
            _queueSize = loggerOptions.BackgroundQueueSize;

            _logMaxLength = CalMaxLength(loggerOptions.LogMaxLength);
            _eventIds = loggerOptions.EventIds;
            _includeException = loggerOptions.IncludeException ?? true;

            Start();
        }
        /// <summary>
        /// 计算最大长度
        /// </summary>
        /// <param name="maxOption">配置的最大长度</param>
        /// <returns>最大长度</returns>
        private int CalMaxLength(int? maxOption)
        {
            int max = int.MaxValue - ContinueString.Length;
            if (maxOption != null && maxOption.Value < max)
            {
                max = maxOption.Value;
            }
            return max;
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="messages"><see cref="LogMessage"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        protected abstract Task WriteMessagesAsync(IEnumerable<LogMessage> messages, CancellationToken token);

        private async Task ProcessLogQueue(object state)
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var limit = _batchSize ?? int.MaxValue;

                while (limit > 0 && _messageQueue.TryTake(out var message))
                {
                    _currentBatch.Add(message);
                    limit--;
                }

                if (_currentBatch.Count > 0)
                {
                    try
                    {
                        await WriteMessagesAsync(_currentBatch, _cancellationTokenSource.Token);
                    }
                    catch
                    {
                        // ignored
                    }

                    _currentBatch.Clear();
                }

                await IntervalAsync(_interval, _cancellationTokenSource.Token);
            }
        }
        /// <summary>
        /// 任务执行间隔
        /// </summary>
        /// <param name="interval">间隔</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        protected virtual Task IntervalAsync(TimeSpan interval, CancellationToken cancellationToken)
        {
            return Task.Delay(interval, cancellationToken);
        }

        internal void AddMessage<TState>(string category, DateTimeOffset timestam, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (_eventIds != null && !_eventIds.Contains(eventId.Id))
            {
                return;
            }
            if (!_messageQueue.IsAddingCompleted)
            {
                try
                {
                    string log = ToLog(category, timestam, logLevel, eventId, state, exception, formatter);
                    _messageQueue.Add(new LogMessage(timestam, log), _cancellationTokenSource.Token);
                }
                catch
                {
                    //cancellation token canceled or CompleteAdding called
                }
            }
        }

        /// <summary>
        /// 转换为日志消息
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="category">日志类别</param>
        /// <param name="timestamp"><see cref="DateTimeOffset"/></param>
        /// <param name="logLevel"><see cref="LogLevel"/></param>
        /// <param name="eventId"><see cref="EventId"/></param>
        /// <param name="state"></param>
        /// <param name="exception">异常</param>
        /// <param name="formatter">格式化器</param>
        protected virtual string ToLog<TState>(string category, DateTimeOffset timestamp, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (_logMaxLength <= 0)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder(128);
            try
            {
                builder.Append(timestamp.ToString("yyyy-MM-dd HH:mm:ss.ffff zzz"));
                builder.Append(" [");
                builder.Append(logLevel.ToString());
                builder.Append("] ");
                builder.Append(category);
                builder.Append("[");
                builder.Append(eventId.ToString());
                builder.Append("]");
                if (formatter != null)
                {
                    builder.Append(": ");
                    builder.AppendLine(formatter(state, exception));
                }
                if (exception != null && _includeException)
                {
                    builder.AppendLine(exception.ToString());
                }
                int length = builder.Length;
                if (length > _logMaxLength)
                {
                    builder.Remove(_logMaxLength, length - _logMaxLength);
                    builder.Append(ContinueString);
                }
                return builder.ToString();
            }
            catch (ArgumentOutOfRangeException)
            {
                return builder.ToString();
            }
        }

        private void Start()
        {
            _messageQueue = _queueSize == null ?
                new BlockingCollection<LogMessage>(new ConcurrentQueue<LogMessage>()) :
                new BlockingCollection<LogMessage>(new ConcurrentQueue<LogMessage>(), _queueSize.Value);

            _cancellationTokenSource = new CancellationTokenSource();
            _outputTask = Task.Factory.StartNew<Task>(
                ProcessLogQueue,
                null,
                TaskCreationOptions.LongRunning);
        }

        private void Stop()
        {
            _cancellationTokenSource.Cancel();
            _messageQueue.CompleteAdding();

            try
            {
                _outputTask.Wait(_interval);
            }
            catch (TaskCanceledException)
            {
            }
            catch (AggregateException ex) when (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is TaskCanceledException)
            {
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// <see cref="ILoggerProvider.CreateLogger(string)"/>
        /// </summary>
        /// <param name="categoryName">日志类型</param>
        /// <returns>日志记录器</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new BatchingLogger(this, categoryName);
        }
    }
}