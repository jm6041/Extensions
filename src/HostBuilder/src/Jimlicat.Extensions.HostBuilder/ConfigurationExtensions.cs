using System;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.Configuration
{

    /// <summary>
    /// <see cref="IConfiguration"/>扩展
    /// </summary>
    public static class ConfigurationExtensions
    {
        private static readonly char[] separator = { ';' };

        /// <summary>
        /// 获得信任证书的指纹 GetSection("TrustCertificates:Thumbprints")
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>证书指纹集合</returns>
        public static string[] GetTrustCertificatesThumbprints(this IConfiguration configuration)
        {
            string vals = configuration.GetSection("TrustCertificates:Thumbprints").Value;
            if (vals != null)
            {
                return vals.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            }
            return Array.Empty<string>();
        }
        /// <summary>
        /// 获得信任证书的序列号 GetSection("TrustCertificates:SerialNumbers")
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>证书序列号集合</returns>
        public static string[] GetTrustCertificatesSerialNumbers(this IConfiguration configuration)
        {
            string vals = configuration.GetSection("TrustCertificates:SerialNumbers").Value;
            if (vals != null)
            {
                return vals.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            }
            return Array.Empty<string>();
        }
        /// <summary>
        /// 获得链接字符串 GetSection("db:ConnectionName").Value ?? entryName
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetConnectionString(this IConfiguration configuration)
        {
            // 入口程序集名
            var entryName = HostHelper.EntryAssemblyName;
            // 获得连接字符串
            string connectionString = configuration.GetConnectionString(configuration.GetSection("db:ConnectionName").Value ?? entryName);
            return connectionString;
        }

        /// <summary>
        /// 获得 Grpc 网关 GetSection("Grpc:Gateway")
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static Uri? GetGrpcGateway(this IConfiguration configuration)
        {
            // 获得 Grpc 网关
            string gw = configuration.GetSection("Grpc:Gateway").Value;
            if (string.IsNullOrEmpty(gw))
            {
                return null;
            }
            return new Uri(gw);
        }

        /// <summary>
        /// 获得配置内容
        /// </summary>
        /// <param name="configuration"></param>
        public static string GetContent(this IConfiguration configuration)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Time: ").Append(DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz")).AppendLine();
            b.Append("MachineName: ").Append(Environment.MachineName).AppendLine();
            b.Append("UserName: ").Append(Environment.UserName).AppendLine();
            b.Append("UserDomainName: ").Append(Environment.UserDomainName).AppendLine();
            b.Append("OSVersion: ").Append(Environment.OSVersion.VersionString).AppendLine();
            b.Append("Runtime: ").Append(Environment.Version).AppendLine();
            b.AppendLine();

            b.Append("Configuration").AppendLine();
            foreach (var c in configuration.AsEnumerable())
            {
                b.Append(c.Key);
                if (!string.IsNullOrEmpty(c.Value))
                {
                    b.Append("=\"").Append(c.Value).Append("\"");
                }
                b.AppendLine();
            }
            b.AppendLine();
            return b.ToString();
        }
    }
}
