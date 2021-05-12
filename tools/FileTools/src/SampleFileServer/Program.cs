using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SampleFileServer
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
#if DEBUG
            HostHelper.IsDebug = true;
#endif
            string contentRoot = HostHelper.GetContentRoot(args);
            // 日志目录
            string logsDir = HostHelper.GetDefaultLogDirectory(contentRoot);
            // 状态文件
            string statusFileName = HostHelper.GetFileName(logsDir, "status");
            // 系统崩溃错误文件
            string errorFileName = HostHelper.GetFileName(logsDir, "error");
            try
            {
                IHostBuilder hostBuilder = HostHelper.CreateHostBuilder(args, statusFileName)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
                var host = hostBuilder.Build();
                HostHelper.WriteStartupLog(args, statusFileName);
                host.Run();
            }
            catch (Exception ex)
            {
                HostHelper.WriteErrorLog(ex, errorFileName);
                throw;
            }
            finally
            {
                HostHelper.WriteExitLog(statusFileName);
            }
        }
    }
}
