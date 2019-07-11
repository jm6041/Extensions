// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// 参考 Microsoft.Extensions.Logging.AzureAppServices.Internal.BatchingLoggerOptions 改写

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Eson.Extensions.Logging
{
    /// <summary>
    /// 参考 Microsoft.Extensions.Logging.AzureAppServices.Internal.BatchingLoggerOptions 改写
    /// </summary>
    public class BatchingLoggerOptions
    {
        private int? _batchSize = 32;
        private int? _backgroundQueueSize;
        private TimeSpan _flushPeriod = TimeSpan.FromSeconds(1);
        private int? _logMaxLength;
        private ICollection<int> _eventIds;
        private bool? _includeException;

        /// <summary>
        /// Gets or sets the period after which logs will be flushed to the store.
        /// </summary>
        public TimeSpan FlushPeriod
        {
            get { return _flushPeriod; }
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(FlushPeriod)} must be positive.");
                }
                _flushPeriod = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum size of the background log message queue or null for no limit.
        /// After maximum queue size is reached log event sink would start blocking.
        /// Defaults to <c>null</c>.
        /// </summary>
        public int? BackgroundQueueSize
        {
            get { return _backgroundQueueSize; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(BackgroundQueueSize)} must be non-negative.");
                }
                _backgroundQueueSize = value;
            }
        }

        /// <summary>
        /// Gets or sets a maximum number of events to include in a single batch or null for no limit.
        /// </summary>
        public int? BatchSize
        {
            get { return _batchSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(BatchSize)} must be positive.");
                }
                _batchSize = value;
            }
        }

        /// <summary>
        /// Gets or sets a maximum length of log message or null for no limit.
        /// </summary>
        public int? LogMaxLength
        {
            get { return _logMaxLength; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(LogMaxLength)} must be positive.");
                }
                _logMaxLength = value;
            }
        }

        /// <summary>
        /// 日志事件ID集合
        /// </summary>
        public ICollection<int> EventIds
        {
            get { return _eventIds; }
            set { _eventIds = value; }
        }
        /// <summary>
        /// 是否包含异常信息
        /// </summary>
        public bool? IncludeException
        {
            get { return _includeException; }
            set { _includeException = value; }
        }

        ///// <summary>
        ///// Gets or sets value indicating if logger accepts and queues writes.
        ///// </summary>
        //public bool IsEnabled { get; set; }
    }
}