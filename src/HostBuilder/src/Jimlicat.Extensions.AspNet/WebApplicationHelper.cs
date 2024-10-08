using System;
using System.IO;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// <see cref="WebApplication"/>帮助类
    /// </summary>
    public static class WebApplicationHelper
    {
        /// <summary>
        /// 获得自定义配置文件 <see cref="HostHelper.GetCustomConfigJsonFile(string, DirectoryInfo?)"/>
        /// </summary>
        /// <param name="contentRoot"></param>
        /// <param name="endDir"></param>
        /// <returns></returns>
        public static string GetCustomConfigJsonFile(string contentRoot, DirectoryInfo? endDir)
        {
            return HostHelper.GetCustomConfigJsonFile(contentRoot, endDir);
        }
        /// <summary>
        /// 参数获得指定配置文件 <see cref="HostHelper.GetArgsConfigJsonFile(string[])"/>
        /// </summary>
        /// <param name="args">启动参数</param>
        /// <returns></returns>
        public static string GetArgsConfigJsonFile(string[] args)
        {
            return HostHelper.GetArgsConfigJsonFile(args);
        }
        /// <summary>
        /// 创建 <see cref="WebApplicationBuilder"/>
        /// </summary>
        /// <param name="args"></param>
        /// <param name="startupStatusFile"></param>
        /// <param name="loggerAction"></param>
        /// <returns></returns>
        public static WebApplicationBuilder CreateBuilder(string[] args, string startupStatusFile, Action<Serilog.LoggerConfiguration>? loggerAction = null)
        {
            // 内容根目录
            string contentRoot = ContentRoot;
            DirectoryInfo? endDir = EndDirInfo;
            // 自定义配置文件
            string customConfigJsonFile = GetCustomConfigJsonFile(contentRoot, endDir);
            // 参数指定配置文件
            string argsConfigJsonFile = GetArgsConfigJsonFile(args);
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { Args = args, ContentRootPath = contentRoot });
            HostHelper.ConfigurationHostBuilder(builder.Host, startupStatusFile, contentRoot, customConfigJsonFile, args, argsConfigJsonFile, endDir, loggerAction);
            return builder;
        }
        /// <summary>
        /// 是否为调试模式，在入口处设置 <see cref="HostHelper.IsDebug"/>
        /// <c>
        /// #if DEBUG
        /// #endif
        /// </c>
        /// </summary>
        public static bool IsDebug
        {
            get => HostHelper.IsDebug;
            set => HostHelper.IsDebug = value;
        }
        /// <summary>
        /// <see cref="HostHelper.GetContentRoot(string[])"/>
        /// </summary>
        /// <param name="args">启动参数</param>
        /// <returns>ContentRoot</returns>
        public static string GetContentRoot(string[] args) => HostHelper.GetContentRoot(args);
        /// <summary>
        /// <see cref="HostHelper.InitContentRoot(string[])"/>
        /// </summary>
        /// <param name="args">启动参数</param>
        /// <returns>ContentRoot</returns>
        public static string InitContentRoot(string[] args)
        {
            return HostHelper.InitContentRoot(args);
        }
        /// <summary>
        /// <see cref="HostHelper.InitContentRoot(string?, string[], DirectoryInfo?)"/>
        /// </summary>
        /// <param name="cr">指定的 ContentRoot</param>
        /// <param name="args">启动参数</param>
        /// <param name="endDir">递归向上查找文件的结束目录</param>
        /// <returns>ContentRoot</returns>
        public static string InitContentRoot(string? cr, string[] args, DirectoryInfo? endDir = null)
        {
            return HostHelper.InitContentRoot(cr, args, endDir);
        }
        /// <summary>
        /// <see cref="HostHelper.ContentRoot"/>
        /// </summary>
        /// <returns>ContentRoot</returns>
        public static string ContentRoot => HostHelper.ContentRoot;
        /// <summary>
        /// <see cref="HostHelper.AppsettingsJsonFile"/>
        /// </summary>
        public static string AppsettingsJsonFile => HostHelper.AppsettingsJsonFile;
        /// <summary>
        /// <see cref="HostHelper.EndDirInfo"/>
        /// </summary>
        /// <returns>ContentRoot</returns>
        public static DirectoryInfo? EndDirInfo => HostHelper.EndDirInfo;
        /// <summary>
        /// 默认日志目录, {contentRoot}/logs/ <see cref="HostHelper.GetDefaultLogDirectory(string)"/>
        /// </summary>
        /// <param name="contentRoot"></param>
        /// <returns></returns>
        public static string GetDefaultLogDirectory(string contentRoot)
        {
            return HostHelper.GetDefaultLogDirectory(contentRoot);
        }
        /// <summary>
        /// 获得指定前缀的文件名 <see cref="HostHelper.GetFileName(string, string, int)"/>
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="prefix"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GetFileName(string dir, string prefix, int count = 3)
        {
            return HostHelper.GetFileName(dir, prefix, count);
        }
        /// <summary>
        /// <see cref="HostHelper.WriteStartupLog(string[], string)"/>
        /// </summary>
        /// <param name="args"></param>
        /// <param name="file"></param>
        public static void WriteStartupLog(string[] args, string file)
        {
            HostHelper.WriteStartupLog(args, file);
        }
        /// <summary>
        /// <see cref="HostHelper.WriteErrorLog(Exception, string)"/>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="file"></param>
        public static void WriteErrorLog(Exception ex, string file)
        {
            HostHelper.WriteErrorLog(ex, file);
        }
        /// <summary>
        /// <see cref="HostHelper.WriteExitLog(string)"/>
        /// </summary>
        /// <param name="file"></param>
        public static void WriteExitLog(string file)
        {
            HostHelper.WriteExitLog(file);
        }
    }
}
