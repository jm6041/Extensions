using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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
            var handler = CreateTrustCertificateHttpClientHandler(trustSerialNumbers, trustThumbprints);
            return handler;
        }
        /// <summary>
        /// 创建信任证书的 HttpClientHandler 根据配置信任证书配置信息
        /// Debug模式，默认信任 CN=localhost 自签名证书
        /// </summary>
        /// <param name="trustSerialNumbers">信任证书序列号</param>
        /// <param name="trustThumbprints">信任证书指纹</param>
        /// <returns></returns>
        public static HttpClientHandler CreateTrustCertificateHttpClientHandler(ICollection<string>? trustSerialNumbers, ICollection<string>? trustThumbprints)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    if (errors == SslPolicyErrors.None)
                    {
                        return true;
                    }
                    if (cert == null)
                    {
                        return false;
                    }
                    if (HostHelper.IsDebug)
                    {
                        // 信任DEBUG模式下的 CN=localhost 自签名证书
                        if (cert.Issuer == cert.Subject && cert.Issuer == "CN=localhost")
                        {
                            return true;
                        }
                    }
                    bool trust = false;
                    if (trustSerialNumbers != null)
                    {
                        trust = trustSerialNumbers.Any(x => x.Equals(cert.SerialNumber, StringComparison.OrdinalIgnoreCase));
                    }
                    if (!trust && trustThumbprints != null)
                    {
                        trust = trustThumbprints.Any(x => x.Equals(cert.Thumbprint, StringComparison.OrdinalIgnoreCase));
                    }
                    return trust;
                }
            };
            return handler;
        }
    }
}
