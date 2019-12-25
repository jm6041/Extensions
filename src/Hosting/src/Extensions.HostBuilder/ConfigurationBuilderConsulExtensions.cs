using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;
using Winton.Extensions.Configuration.Consul;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// K8s配置扩展
    /// </summary>
    internal static class ConfigurationBuilderK8sExtensions
    {
        /// <summary>
        /// 添加Consul配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="consulUri">consulUri</param>
        /// <param name="sharedKey">共享的Key</param>
        /// <param name="specialKey">特定的Key</param>
        /// <param name="args">参数</param>
        public static void AddConsulConfiguration(this IConfigurationBuilder builder, string sharedKey, string specialKey, string consulUri, string[] args)
        {
            builder.AddMyConsul(sharedKey, consulUri).AddMyConsul(specialKey, consulUri);
            builder.AddEnvironmentVariables();
            if (args != null)
            {
                builder.AddCommandLine(args);
            }
        }

        /// <summary>
        /// 添加Consul配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="key"></param>
        /// <param name="consulUri"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddMyConsul(this IConfigurationBuilder builder, string key, string consulUri)
        {
            builder.AddConsul(key,
                options =>
                {
                    options.ConsulConfigurationOptions =
                            cco => { cco.Address = new Uri(consulUri); };
                    options.Optional = false;
                    options.ReloadOnChange = true;
                    options.OnLoadException = exceptionContext => { exceptionContext.Ignore = false; };
                    options.Parser = new Winton.Extensions.Configuration.Consul.Parsers.JsonConfigurationParser();
                });
            return builder;
        }
    }
}
