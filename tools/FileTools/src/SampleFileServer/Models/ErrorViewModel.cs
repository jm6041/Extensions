using System;

namespace SampleFileServer.Models
{
    /// <summary>
    /// 异常试图模型
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// 请求Id
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// 是否显示请求Id
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
