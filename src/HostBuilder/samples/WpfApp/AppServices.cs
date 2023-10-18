using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WpfApp
{
    /// <summary>
    /// App 依赖服务
    /// </summary>
    public static class AppServices
    {
        /// <summary>
        /// 程序集入口名
        /// </summary>
        public static readonly string AssemblyEntryName = typeof(App).Assembly.GetName().Name ?? "WpfApp";
        /// <summary>
        /// 应用基础目录目录
        /// </summary>
        public static readonly DirectoryInfo AppBaseDirInfo;
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static AppServices()
        {
            AppBaseDirInfo = Directory.CreateDirectory(AppContext.BaseDirectory);
        }
        /// <summary>
        /// IServiceProvider
        /// </summary>
        public static IServiceProvider Services { get; private set; } = null!;
        /// <summary>
        /// 初始化
        /// </summary>
        public static IServiceProvider Init()
        {
#if DEBUG
            HostHelper.IsDebug = true;
#endif
            var entryName = AssemblyEntryName;
            HostHelper.EntryAssemblyName = entryName;
            var arrEmpty = Array.Empty<string>();
            var mainDir = AppBaseDirInfo.CreateSubdirectory(entryName);
            // 内容根目录
            string contentRoot = HostHelper.InitContentRoot(mainDir.FullName, arrEmpty, AppBaseDirInfo);
            // 日志目录
            string logsDir = HostHelper.GetDefaultLogDirectory(contentRoot);
            // 状态文件
            string statusFileName = HostHelper.GetFileName(logsDir, "status");
            // 系统崩溃错误文件
            string errorFileName = HostHelper.GetFileName(logsDir, "error");
            try
            {
                IHostBuilder hostBuilder = HostHelper.CreateHostBuilder(arrEmpty, statusFileName);
                hostBuilder.ConfigureAppConfiguration((context, configBuilder) =>
                {
                }).ConfigureServices((context, services) =>
                {
                    services.ConfigServices(context);
                });
                var _host = hostBuilder.Build();
                HostHelper.WriteStartupLog(arrEmpty, statusFileName);
                _host.Start();
                var config = _host.Services.GetRequiredService<IConfiguration>();
                using (var sw = File.AppendText(statusFileName))
                {
                    sw.WriteLine(config.GetContent());
                }
                Services = _host.Services;
                return Services;
            }
            catch (Exception ex)
            {
                HostHelper.WriteErrorLog(ex, errorFileName);
                throw;
            }
        }
        /// <summary>
        /// 配置依赖
        /// </summary>
        /// <param name="services"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigServices(this IServiceCollection services, HostBuilderContext context)
        {
            services.AddTransient<MainWindow>();
            return services;
        }
    }
}
