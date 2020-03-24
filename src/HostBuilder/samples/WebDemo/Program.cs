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
            string contentRoot = P.GetContentRoot(args);
            // 日志目录
            string logsDir = P.GetDefaultLogDirectory(contentRoot);
            // 状态文件
            string statusFileName = P.GetFileName(logsDir, "status");
            // 系统崩溃错误文件
            string errorFileName = P.GetFileName(logsDir, "error");
            try
            {
                IHostBuilder hostBuilder = P.CreateHostBuilder(args, statusFileName);
                hostBuilder.ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
                var host = hostBuilder.Build();
                P.WriteStartupLog(args, statusFileName);
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
