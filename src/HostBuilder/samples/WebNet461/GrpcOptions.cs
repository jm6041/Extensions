using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNet461
{
    /// <summary>
    /// Grpc配置，仅仅用于传统的Grpc.Core配置
    /// </summary>
    public class GrpcOptions
    {
        /// <summary>
        /// 主机名
        /// </summary>
        public string Host { get; set; } = "localhost";
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 80;
        /// <summary>
        /// 服务端证书文件，此证书不含有私钥
        /// </summary>
        public string ServerCertificateFile { get; set; }
        /// <summary>
        /// 客户端证书文件
        /// </summary>
        public string ClientCertificateFile { get; set; }
        /// <summary>
        /// 客服端证书文件私钥
        /// </summary>
        public string ClientCertificateKeyFile { get; set; }
    }
}
