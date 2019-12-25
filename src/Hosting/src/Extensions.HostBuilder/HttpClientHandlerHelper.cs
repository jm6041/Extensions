using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace System.Net.Http
{
    /// <summary>
    /// <see cref="HttpClientHandler"/>帮助类
    /// </summary>
    public static class HttpClientHandlerHelper
    {
        /// <summary>
        /// 信任证书的 HttpClientHandler
        /// </summary>
        /// <returns></returns>
        public static HttpClientHandler TrustCertificateHttpClientHandler(IServiceProvider serviceProvider)
        {
            var trust = serviceProvider.GetRequiredService<IConfiguration>().GetTrustCertificatesThumbprints();
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
#if DEBUG
                    // 信任DEBUG模式下的 CN=localhost 自签名证书
                    if (cert.Issuer == cert.Subject && cert.Issuer == "CN=localhost")
                    {
                        return true;
                    }
#endif
                    return trust.Any(x => x.Equals(cert.Thumbprint, StringComparison.OrdinalIgnoreCase));
                }
            };
            return handler;
        }
    }
}
