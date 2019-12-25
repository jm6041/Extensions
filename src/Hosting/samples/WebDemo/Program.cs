using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebDemo
{
    /// <summary>
    /// Consul常量
    /// </summary>
    internal class ConsulConst
    {
        /// <summary>
        /// 共享Key
        /// </summary>
        public const string SharedKey = "ctc/appsettings/shared";
        /// <summary>
        /// 特定Key
        /// </summary>
        public const string SpecialKey = "ctc/appsettings/IdentityServer";
    }

    /// <summary>
    /// 入口程序
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 入口方法
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            string contentRoot = P.GetContentRoot();
            // 日志目录
            string logsDir = Path.Combine(contentRoot, "logs");
            Directory.CreateDirectory(logsDir);
            // 状态文件
            string statusFileName = P.GetFileName(logsDir, "status");
            // 系统崩溃错误文件
            string errorFileName = P.GetFileName(logsDir, "error");
            try
            {
                IHostBuilder hostBuilder = P.CreateHostBuilder(args, statusFileName, ConsulConst.SharedKey, ConsulConst.SpecialKey);
                hostBuilder.ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
                var host = hostBuilder.Build();
                P.WriteStartupLog(args, statusFileName, ConsulConst.SharedKey, ConsulConst.SpecialKey);
                host.Run();
            }
            catch (Exception ex)
            {
                P.WriteErrorLog(ex, errorFileName);
                throw;
            }
            finally
            {
                P.WriteExitLog(statusFileName);
            }
        }
    }
}
