using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace System.Net.Http
{
    /// <summary>
    /// <see cref="HttpClientHandler"/>帮助类
    /// </summary>
    public static class HttpClientHandlerHelper
    {
        /// <summary>
        /// 创建信任证书的 HttpClientHandler 根据配置信任证书配置信息
        /// Debug模式，默认信任 CN=localhost 自签名证书
        /// </summary>
        /// <returns></returns>
        public static HttpClientHandler CreateTrustCertificateHttpClientHandler(IConfiguration configuration)
        {
            var trustSerialNumbers = configuration.GetTrustCertificatesSerialNumbers();
            var trustThumbprints = configuration.GetTrustCertificatesThumbprints();
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    if (HostHelper.IsDebug)
                    {
                        // 信任DEBUG模式下的 CN=localhost 自签名证书
                        if (cert.Issuer == cert.Subject && cert.Issuer == "CN=localhost")
                        {
                            return true;
                        }
                    }
                    return trustSerialNumbers.Any(x => x.Equals(cert.SerialNumber, StringComparison.OrdinalIgnoreCase))
                    || trustThumbprints.Any(x => x.Equals(cert.Thumbprint, StringComparison.OrdinalIgnoreCase));
                }
            };
            return handler;
        }
    }
}
