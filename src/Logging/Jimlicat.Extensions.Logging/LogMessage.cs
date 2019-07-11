// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// 参考 Microsoft.Extensions.Logging.AzureAppServices.Internal.LogMessage 改写

using System;

namespace Eson.Extensions.Logging
{
    /// <summary>
    /// 日志结构
    /// 参考 Microsoft.Extensions.Logging.AzureAppServices.Internal.LogMessage 改写
    /// </summary>
    public struct LogMessage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="timestamp">时间</param>
        /// <param name="message">日志内容</param>
        public LogMessage(DateTimeOffset timestamp, string message)
        {
            Timestamp = timestamp;
            Message = message;
        }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTimeOffset Timestamp { get; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Message { get; }
    }
}