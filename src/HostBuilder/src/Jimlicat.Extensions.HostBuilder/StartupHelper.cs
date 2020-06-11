using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace System
{
    /// <summary>
    /// Startup相关帮助信息
    /// </summary>
    public static class StartupHelper
    {
        /// <summary>
        /// 获得系统信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
#if NETCOREAPP2_1 || NET461
        public static string GetSystemInfo(HttpRequest request, IHostingEnvironment env, IConfiguration configuration)
        {
            var b = GetSystemInfo(request, configuration, env.ApplicationName, env.EnvironmentName, env.ContentRootPath, env.WebRootPath);
            return b.ToString();
        }
#else
        public static string GetSystemInfo(HttpRequest request, IWebHostEnvironment env, IConfiguration configuration)
        {
            var b = GetSystemInfo(request, configuration, env.ApplicationName, env.EnvironmentName, env.ContentRootPath, env.WebRootPath);
            return b.ToString();
        }
#endif

        /// <summary>
        /// 获得系统信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="configuration"></param>
        /// <param name="appName"></param>
        /// <param name="environmentName"></param>
        /// <param name="contentRootPath"></param>
        /// <param name="webRootPath"></param>
        /// <returns></returns>
        private static StringBuilder GetSystemInfo(HttpRequest request, IConfiguration configuration, string appName, string environmentName, string contentRootPath, string webRootPath)
        {
            StringBuilder b = new StringBuilder();
            b.Append(appName).AppendLine();
            b.Append("Time: ").Append(DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz")).AppendLine();
            b.Append("Host: ").Append(request.Host).AppendLine();
            b.Append("Remote: ").Append(request.HttpContext.Connection.RemoteIpAddress).Append(":").Append(request.HttpContext.Connection.RemotePort).AppendLine();
            b.Append("Local: ").Append(request.HttpContext.Connection.LocalIpAddress).Append(":").Append(request.HttpContext.Connection.LocalPort).AppendLine();
            b.Append("MachineName: ").Append(Environment.MachineName).AppendLine();
            b.Append("UserName: ").Append(Environment.UserName).AppendLine();
            b.Append("UserDomainName: ").Append(Environment.UserDomainName).AppendLine();
            b.Append("OSVersion: ").Append(Environment.OSVersion.VersionString).AppendLine();
            b.Append("WebHostEnvironment").AppendLine();
            b.Append("Runtime: ").Append(Environment.Version).AppendLine();
            b.Append("EnvironmentName: ").Append(environmentName).AppendLine();
            b.Append("ContentRootPath: ").Append(contentRootPath).AppendLine();
            b.Append("WebRootPath: ").Append(webRootPath).AppendLine();

            b.AppendLine();
            b.Append("Configuration").AppendLine();

            if (HostHelper.IsDebug)
            {
                foreach (var c in configuration.AsEnumerable())
                {
                    b.Append(c.Key);
                    if (!string.IsNullOrEmpty(c.Value))
                    {
                        b.Append("=\"").Append(c.Value).Append("\"");
                    }
                    b.AppendLine();
                }
            }
            else
            {
                foreach (var c in configuration.GetSection("Kestrel:EndPoints").AsEnumerable())
                {
                    if (c.Key.EndsWith("Url", StringComparison.OrdinalIgnoreCase))
                    {
                        b.Append(c.Key).Append("=\"").Append(c.Value).AppendLine("\"");
                    }
                }
            }
            return b;
        }

        /// <summary>
        /// 写入连接字符串
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        public static void WriteConnectionString(string conn, IConfiguration configuration)
        {
            StringBuilder b = new StringBuilder("ConnectionString: ");
            if (!string.IsNullOrEmpty(conn))
            {
                b.Append("\"").Append(conn).Append("\"");
            }
            b.AppendLine();

            string file = configuration.GetSection(M.StartupStatusFileKey)?.Value;
            if (!string.IsNullOrEmpty(file))
            {
                using (var streamWriter = File.AppendText(file))
                {
                    streamWriter.WriteLine(b.ToString());
                }
            }
            Console.WriteLine(b.ToString());
        }

        /// <summary>
        /// 写入配置信息
        /// </summary>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
#if NETCOREAPP2_1 || NET461
        public static void WriteConfiguration(IHostingEnvironment env, IConfiguration configuration)
        {
            var config = GetConfigurationInner(configuration, env.ApplicationName, env.EnvironmentName, env.ContentRootPath, env.WebRootPath);
            WriteConfigurationInner(configuration, config);
        }
#else
        public static void WriteConfiguration(IWebHostEnvironment env, IConfiguration configuration)
        {
            var config = GetConfigurationInner(configuration, env.ApplicationName, env.EnvironmentName, env.ContentRootPath, env.WebRootPath);
            WriteConfigurationInner(configuration, config);
        }
#endif
        /// <summary>
        /// Configuration EventId
        /// </summary>
        public static readonly EventId ConfigurationEventId = new EventId(19, "Configuration");
        /// <summary>
        /// 日志记录配置信息
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        /// <param name="logLevel"></param>
#if NETCOREAPP2_1 || NET461
        public static void LogConfiguration(ILogger logger, IHostingEnvironment env, IConfiguration configuration, LogLevel logLevel = LogLevel.Information)
        {
            var config = GetConfigurationInner(configuration, env.ApplicationName, env.EnvironmentName, env.ContentRootPath, env.WebRootPath);
            LogConfigurationInner(logger, config, logLevel);
        }
#else
        public static void LogConfiguration(ILogger logger, IWebHostEnvironment env, IConfiguration configuration, LogLevel logLevel = LogLevel.Information)
        {
            var config = GetConfigurationInner(configuration, env.ApplicationName, env.EnvironmentName, env.ContentRootPath, env.WebRootPath);
            LogConfigurationInner(logger, config, logLevel);
        }
#endif
        private static void LogConfigurationInner(ILogger logger,  string config, LogLevel logLevel)
        {
            logger.Log(logLevel, ConfigurationEventId, config);
        }

        /// <summary>
        /// 写入并且日志记录配置信息
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        /// <param name="logLevel"></param>
#if NETCOREAPP2_1 || NET461
        public static void WriteAndLogConfiguration(ILogger logger, IHostingEnvironment env, IConfiguration configuration, LogLevel logLevel = LogLevel.Information)
        {
            var config = GetConfigurationInner(configuration, env.ApplicationName, env.EnvironmentName, env.ContentRootPath, env.WebRootPath);
            WriteConfigurationInner(configuration, config);
            LogConfigurationInner(logger, config, logLevel);
        }
#else
        public static void WriteAndLogConfiguration(ILogger logger, IWebHostEnvironment env, IConfiguration configuration, LogLevel logLevel = LogLevel.Information)
        {
            var config = GetConfigurationInner(configuration, env.ApplicationName, env.EnvironmentName, env.ContentRootPath, env.WebRootPath);
            WriteConfigurationInner(configuration, config);
            LogConfigurationInner(logger, config, logLevel);
        }
#endif
        /// <summary>
        /// 获得配置信息
        /// </summary>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
#if NETCOREAPP2_1 || NET461
        public static string GetConfiguration(IHostingEnvironment env, IConfiguration configuration)
        {
            var config = GetConfigurationInner(configuration, env.ApplicationName, env.EnvironmentName, env.ContentRootPath, env.WebRootPath);
            return config;
        }
#else
        public static string GetConfiguration(IWebHostEnvironment env, IConfiguration configuration)
        {
            var config = GetConfigurationInner(configuration, env.ApplicationName, env.EnvironmentName, env.ContentRootPath, env.WebRootPath);
            return config;
        }
#endif
        private static void WriteConfigurationInner(IConfiguration configuration, string config)
        {
            string file = configuration.GetSection(M.StartupStatusFileKey)?.Value;
            if (!string.IsNullOrEmpty(file))
            {
                using (var streamWriter = File.AppendText(file))
                {
                    streamWriter.WriteLine(config);
                }
            }
            Console.WriteLine(config);
        }

        /// <summary>
        /// 获得配置信息
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="appName"></param>
        /// <param name="environmentName"></param>
        /// <param name="contentRootPath"></param>
        /// <param name="webRootPath"></param>
        private static string GetConfigurationInner(IConfiguration configuration, string appName, string environmentName, string contentRootPath, string webRootPath)
        {
            StringBuilder b = new StringBuilder();
            b.Append(appName).AppendLine();
            b.Append("Time: ").Append(DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz")).AppendLine();
            b.Append("MachineName: ").Append(Environment.MachineName).AppendLine();
            b.Append("UserName: ").Append(Environment.UserName).AppendLine();
            b.Append("UserDomainName: ").Append(Environment.UserDomainName).AppendLine();
            b.Append("OSVersion: ").Append(Environment.OSVersion.VersionString).AppendLine();
            b.Append("WebHostEnvironment").AppendLine();
            b.Append("Runtime: ").Append(Environment.Version).AppendLine();
            b.Append("EnvironmentName: ").Append(environmentName).AppendLine();
            b.Append("ContentRootPath: ").Append(contentRootPath).AppendLine();
            b.Append("WebRootPath: ").Append(webRootPath).AppendLine();

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
