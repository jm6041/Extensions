// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// �ο� Microsoft.Extensions.Logging.AzureAppServices.Internal.LogMessage ��д

using System;

namespace Eson.Extensions.Logging
{
    /// <summary>
    /// ��־�ṹ
    /// �ο� Microsoft.Extensions.Logging.AzureAppServices.Internal.LogMessage ��д
    /// </summary>
    public struct LogMessage
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="timestamp">ʱ��</param>
        /// <param name="message">��־����</param>
        public LogMessage(DateTimeOffset timestamp, string message)
        {
            Timestamp = timestamp;
            Message = message;
        }

        /// <summary>
        /// ʱ��
        /// </summary>
        public DateTimeOffset Timestamp { get; }
        /// <summary>
        /// ��־����
        /// </summary>
        public string Message { get; }
    }
}