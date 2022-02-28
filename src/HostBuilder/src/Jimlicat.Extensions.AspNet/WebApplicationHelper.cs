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
        /// <returns></returns>
        public static WebApplicationBuilder CreateBuilder(string[] args, string startupStatusFile)
        {
            // 内容根目录
            string contentRoot = GetContentRoot();
            DirectoryInfo? endDir = GetEndDirInfo();
            // 自定义配置文件
            string customConfigJsonFile = GetCustomConfigJsonFile(contentRoot, endDir);
            // 参数指定配置文件
            string argsConfigJsonFile = GetArgsConfigJsonFile(args);
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { Args = args, ContentRootPath = contentRoot });
            HostHelper.ConfigurationHostBuilder(builder.Host, startupStatusFile, contentRoot, customConfigJsonFile, args, argsConfigJsonFile, endDir);
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
        /// <see cref="HostHelper.GetContentRoot()"/>
        /// </summary>
        /// <returns>ContentRoot</returns>
        public static string GetContentRoot()
        {
            return HostHelper.GetContentRoot();
        }
        /// <summary>
        /// 获得 ContentRoot，必须先调用 InitContentRoot
        /// </summary>
        /// <param name="args"></param>
        /// <returns>ContentRoot</returns>
        [Obsolete("Please use InitContentRoot", false)]
        public static string GetContentRoot(string[] args)
        {
            return InitContentRoot(args);
        }
        /// <summary>
        /// <see cref="HostHelper.GetEndDirInfo"/>
        /// </summary>
        /// <returns>ContentRoot</returns>
        public static DirectoryInfo? GetEndDirInfo()
        {
            return HostHelper.GetEndDirInfo();
        }
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
