// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// 参考 Microsoft.Extensions.Logging.AzureAppServices.Internal.BatchingLogger 改写

using System;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Eson.Extensions.Logging
{
    /// <summary>
    /// 批量日志记录器
    /// 参考 Microsoft.Extensions.Logging.AzureAppServices.Internal.BatchingLogger 改写
    /// </summary>
    public class BatchingLogger : ILogger
    {
        private readonly BatchingLoggerProvider _provider;
        private readonly string _category;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="loggerProvider"><see cref="BatchingLoggerProvider"/></param>
        /// <param name="categoryName">日志类型</param>
        public BatchingLogger(BatchingLoggerProvider loggerProvider, string categoryName)
        {
            _provider = loggerProvider;
            _category = categoryName;
        }

        /// <summary>
        /// <see cref="ILogger.BeginScope{TState}(TState)"/>
        /// 不支持
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
        /// <summary>
        /// <see cref="ILogger.IsEnabled(LogLevel)"/>
        /// </summary>
        /// <param name="logLevel"><see cref="LogLevel"/></param>
        /// <returns></returns>
        public virtual bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel == LogLevel.None)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="timestamp"><see cref="DateTimeOffset"/></param>
        /// <param name="logLevel"><see cref="LogLevel"/></param>
        /// <param name="eventId"><see cref="EventId"/></param>
        /// <param name="state"></param>
        /// <param name="exception">异常</param>
        /// <param name="formatter">格式化器</param>
        private void Log<TState>(DateTimeOffset timestamp, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            _provider.AddMessage(_category, timestamp, logLevel, eventId, state, exception, formatter);
        }
        /// <summary>
        /// <see cref="ILogger.Log{TState}(LogLevel, EventId, TState, Exception, Func{TState, Exception, string})"/>
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"><see cref="LogLevel"/></param>
        /// <param name="eventId"><see cref="EventId"/></param>
        /// <param name="state"></param>
        /// <param name="exception">异常</param>
        /// <param name="formatter">格式化器</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Log(DateTimeOffset.Now, logLevel, eventId, state, exception, formatter);
        }
    }
}
