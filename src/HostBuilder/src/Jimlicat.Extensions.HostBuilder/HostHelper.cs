using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.WindowsServices;
using Serilog;
using Serilog.Events;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 与系统启动相关帮助函数
    /// </summary>
    public static class HostHelper
    {
        /// <summary>
        /// 启动状态文件配置Key
        /// </summary>
        public const string StartupStatusFileKey = "StartupStatusFile";
        /// <summary>
        /// 是否Windows服务托管
        /// </summary>
        private static bool IsWindowsService { get; } = WindowsServiceHelpers.IsWindowsService();
        /// <summary>
        /// ContentRoot
        /// </summary>
        private static volatile string? s_ContentRoot;
        /// <summary>
        /// 递归向上查找文件的结束目录
        /// </summary>
        private static volatile DirectoryInfo? s_EndDirInfo;
        private static volatile bool is_initEndDir = false;

        /// <summary>
        /// 是否为调试模式，在入口处设置
        /// <c>
        /// #if DEBUG
        /// #endif
        /// </c>
        /// </summary>
        public static bool IsDebug { get; set; } = false;
        /// <summary>
        /// 初始化 ContentRoot，必须第一个调用，确定 ContentRoot
        /// </summary>
        /// <param name="args">启动参数</param>
        /// <returns>ContentRoot</returns>
        public static string InitContentRoot(string[] args)
        {
            return InitContentRoot(null, args, null);
        }
        /// <summary>
        /// 初始化 ContentRoot，必须第一个调用，确定 ContentRoot
        /// </summary>
        /// <param name="cr">指定的 ContentRoot</param>
        /// <param name="args">启动参数</param>
        /// <param name="endDir">递归向上查找文件的结束目录</param>
        /// <returns>ContentRoot</returns>
        public static string InitContentRoot(string? cr, string[] args, DirectoryInfo? endDir = null)
        {
            if (s_ContentRoot != null)
            {
                return s_ContentRoot;
            }

            if (!is_initEndDir)
            {
                s_EndDirInfo = endDir;
                is_initEndDir = true;
            }

            if (cr != null && cr.Length != 0)
            {
                var dir = Directory.CreateDirectory(cr).FullName;
                Interlocked.CompareExchange(ref s_ContentRoot, dir, null);
                return s_ContentRoot;
            }
            else
            {

                // 内容根目录，默认当前程序真实目录
                string contentRoot = AppContext.BaseDirectory;
                bool isws = IsWindowsService;
                if (isws)
                {
                    Interlocked.CompareExchange(ref s_ContentRoot, contentRoot, null);
                    return s_ContentRoot;
                }

                // 当前目录，
                string curDir = Directory.GetCurrentDirectory();
                // 当前目录信息
                DirectoryInfo curDirInfo = new DirectoryInfo(curDir);
                // 当前目录包含项目文件，表示从项目启动，例如 dotnet run， 默认使用当前目录
                if (curDirInfo.EnumerateFiles("*.csproj").Any())
                {
                    contentRoot = curDir;
                }
                // 如果启动参数指定目录，使用参数
                var argContentRoot = GetContentRootArg(args);
                if (!string.IsNullOrEmpty(argContentRoot))
                {
                    contentRoot = argContentRoot;
                }
                Interlocked.CompareExchange(ref s_ContentRoot, contentRoot, null);
                return s_ContentRoot;
            }
        }
        /// <summary>
        /// 获得 ContentRoot，必须先调用 InitContentRoot
        /// </summary>
        /// <returns>ContentRoot</returns>
        public static string GetContentRoot()
        {
            if (s_ContentRoot == null)
            {
                throw new InvalidOperationException("Invoke InitContentRoot");
            }
            return s_ContentRoot;
        }
        /// <summary>
        /// 活动从ContentRoot 递归向上查找文件的结束目录， 必须先调用 InitContentRoot
        /// </summary>
        /// <returns>ContentRoot</returns>
        public static DirectoryInfo? GetEndDirInfo()
        {
            if (is_initEndDir == false)
            {
                throw new InvalidOperationException("Invoke InitContentRoot");
            }
            return s_EndDirInfo;
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
        /// 默认日志目录, {contentRoot}/logs/
        /// </summary>
        /// <param name="contentRoot"></param>
        /// <returns></returns>
        public static string GetDefaultLogDirectory(string contentRoot)
        {
            // 日志目录
            string logsDir = Path.Combine(contentRoot, "logs");
            Directory.CreateDirectory(logsDir);
            return logsDir;
        }

        private static string GetContentRootArg(string[] args)
        {
            string cr = "";
            if (args != null)
            {
                int count = args.Length;
                for (int i = 0; i < count - 1; i++)
                {
                    string arg = args[i];
                    if (arg.Equals(M.cr_arg, StringComparison.OrdinalIgnoreCase) || arg.Equals(M.ContentRoot_Arg, StringComparison.OrdinalIgnoreCase))
                    {
                        cr = args[i + 1];
                    }
                }
            }
            return cr;
        }
        /// <summary>
        /// 获得自定义配置文件
        /// </summary>
        /// <param name="contentRoot"></param>
        /// <param name="endDir"></param>
        /// <returns></returns>
        public static string GetCustomConfigJsonFile(string contentRoot, DirectoryInfo? endDir)
        {
            DirectoryInfo dir = new DirectoryInfo(contentRoot);
            if (dir.Parent == null)
            {
                return string.Empty;
            }
            var file = DirectoryHelper.GetPathOfFileAbove("*config.json", dir.Parent, endDir);
            // 自定义配置文件不存在，用空文件代替
            if (file == null)
            {
                file = string.Empty;
            }
            return file;
        }
        /// <summary>
        /// HostBuilder
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="startupStatusFile">启动状态保存文件</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args, string startupStatusFile)
        {
            // 内容根目录
            string contentRoot = GetContentRoot();
            DirectoryInfo? endDir = GetEndDirInfo();
            // 自定义配置文件
            string customConfigJsonFile = GetCustomConfigJsonFile(contentRoot, endDir);
            // 参数指定配置文件
            string argsConfigJsonFile = GetArgsConfigJsonFile(args);
            var hostBuilder = Host.CreateDefaultBuilder(args);
            return ConfigurationHostBuilder(hostBuilder, startupStatusFile, contentRoot, customConfigJsonFile, args, argsConfigJsonFile, endDir);
        }
        /// <summary>
        /// 配置<see cref="IHostBuilder"/>
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="startupStatusFile">启动状态保存文件</param>
        /// <param name="contentRoot"></param>
        /// <param name="customConfigJsonFile">自定义配置文件</param>
        /// <param name="args"></param>
        /// <param name="argsConfigJsonFile">参数指定配置文件</param>
        /// <param name="endDir">递归向上查找文件的结束目录</param>
        /// <returns></returns>
        public static IHostBuilder ConfigurationHostBuilder(IHostBuilder hostBuilder, string startupStatusFile, string contentRoot, string customConfigJsonFile, string[] args, string argsConfigJsonFile, DirectoryInfo? endDir)
        {
            M.TEMP_CONFIG_DIC[M.ContentRootKey] = contentRoot;
            M.TEMP_CONFIG_DIC[M.CustomConfigFileKey] = customConfigJsonFile;
            M.TEMP_CONFIG_DIC[M.ArgsConfigFileKey] = argsConfigJsonFile;
            if (!string.IsNullOrWhiteSpace(startupStatusFile))
            {
                hostBuilder.ConfigureHostConfiguration((config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string>() {
                        { StartupStatusFileKey, startupStatusFile }
                    });
                });
            }
            hostBuilder
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment;
                    builder.AddLocalConfiguration(contentRoot, env.EnvironmentName, customConfigJsonFile, args, argsConfigJsonFile, endDir);
                })
                .ConfigureLogging((hostContext, builder) =>
                {
                    var logger = CreateLogger(contentRoot, hostContext.Configuration);
                    builder.AddSerilog(logger);
                })
                .UseContentRoot(contentRoot);
            return hostBuilder;
        }
        /// <summary>
        /// 是否已经配置了Serilog写文件日志
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static bool HasSerilogWriteToFile(IConfiguration configuration)
        {
            bool hasFile = configuration.GetSection("Serilog:WriteTo").GetChildren().Any(x => x.GetSection("Name").Value == "File");
            return hasFile;
        }

        private static Serilog.ILogger CreateLogger(string contentRoot, IConfiguration configuration)
        {
            LogEventLevel logEventLevel = LogEventLevel.Warning;
            if (IsDebug)
            {
                logEventLevel = LogEventLevel.Debug;
            }
            var logConfig = new LoggerConfiguration();
            if (!HasSerilogWriteToFile(configuration))  // 如果没有配置Serilog写文件日志，添加默认
            {
                string logDir = GetDefaultLogDirectory(contentRoot);
                string entryName = EntryAssemblyName;
                string logPath = Path.Combine(logDir, $"{entryName}-log.log");
                logConfig.WriteTo.File(
                    path: logPath,
                    restrictedToMinimumLevel: logEventLevel,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext:l}] {Message:lj}{NewLine}{Exception}",
                    fileSizeLimitBytes: 20 * 1024 * 1024,
                    flushToDiskInterval: TimeSpan.FromMinutes(1),
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 4);
            }
            return logConfig.ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        /// <summary>
        /// 参数获得指定配置文件
        /// </summary>
        /// <param name="args">启动参数</param>
        /// <returns></returns>
        public static string GetArgsConfigJsonFile(string[] args)
        {
            string file = "";
            if (args != null)
            {
                int count = args.Length;
                for (int i = 0; i < count - 1; i++)
                {
                    string arg = args[i];
                    if (arg.Equals(M.cf_arg, StringComparison.OrdinalIgnoreCase) || arg.Equals(M.ConfigFile_Arg, StringComparison.OrdinalIgnoreCase))
                    {
                        file = args[i + 1];
                    }
                }
            }
            return file;
        }

        private static string? s_entryName = Assembly.GetEntryAssembly()?.GetName().Name;
        /// <summary>
        /// 设置或者获取入口程序集名
        /// </summary>
        /// <returns></returns>
        public static string EntryAssemblyName
        {
            get
            {
                return s_entryName ?? string.Empty;
            }
            set
            {
                if (value != null && value.Length != 0)
                {
                    s_entryName = value;
                }
            }
        }
        /// <summary>
        /// 获得指定前缀的文件名
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="prefix"></param>
        /// <param name="count">保留前缀的文件数量，默认为3</param>
        /// <returns></returns>
        public static string GetFileName(string dir, string prefix, int count = 3)
        {
            string entryName = EntryAssemblyName;
            var start = entryName + "-" + prefix;
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            // 删除早期的文件
            var dfs = dirInfo.EnumerateFiles(start + "-*.log").OrderByDescending(x => x.LastWriteTimeUtc).Skip(count).ToList();
            foreach (var df in dfs)
            {
                df.Delete();
            }
            DateTimeOffset now = DateTimeOffset.Now;
            //string n = string.Format("{0}-{1:yyyyMMdd}-{2}.log", start, now, now.Ticks);
            string n = string.Format("{0}-{1:yyyyMMdd}.log", start, now);
            string fn = Path.Combine(dir, n);
            return fn;
        }
        /// <summary>
        /// 写入启动日志
        /// </summary>
        /// <param name="args">启动参数</param>
        /// <param name="file">写入的文件</param>
        public static void WriteStartupLog(string[] args, string file)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("=============================== Start ===============================");
            sb.AppendLine();
            // 入口程序集名
            var entryName = EntryAssemblyName;
            sb.AppendFormat("{0:yyyy-MM-dd HH:mm:ss.ffff zzz} Program {1} Starting", DateTimeOffset.Now, entryName);
            sb.AppendLine();
            string contentRoot = GetContentRoot();
            sb.AppendFormat($"ContentRoot: \"{contentRoot}\"").AppendLine();
            var endDir = GetEndDirInfo();
            if (endDir != null)
            {
                sb.AppendFormat($"EndDirInfo: \"{endDir.FullName}\"").AppendLine();
            }
            sb.Append("------------------- 配置说明 -------------------");
            sb.AppendLine();
            string localConfigInfo = @"所有的配置都是键值对，在环境变量中使用双下划线'__'代替英文冒号':'，在参数中推荐格式为'--Key Value','-key val'
键值对的值安配置加载顺序，后面的覆盖前面的值
配置加载顺序
  默认配置(详情见 https://docs.microsoft.com/zh-cn/aspnet/)
  默认配置文件 ( ContentRoot 上级目录开始向根目录递归查找，建议存放多个项目通用的配置信息)
    default.json
    default.{EnvironmentName}.json
  config配置 ( ContentRoot 目录中查找，建议存放项目特定信息)
    config.json
    config.ini
    config.{EnvironmentName}.json
    config.{EnvironmentName}.ini
    config.{MachineName}.json
    config.{MachineName}.ini
    config.{UserName}.json
    config.{UserName}.ini
  secrets 目录配置文件 (同类型文件安名字先后循序，建议存放有保密信息的配置，妥善保管，不建议docker镜像包含保密信息，docker容器运行，保密信息可以通过此目录挂载)
    *.json
    *.ini
  config 目录配置文件 (同类型文件安名字先后循序，建议存储除保密信息的外的配置文件，docker容器运行，配置文件可以通过此目录挂载)
    *.json
    *.ini
  自定义配置文件 ( ContentRoot 上级目录开始向根目录递归查找，建议存放多个项目通用的配置信息)
    *config.json
  参数指定配置文件 ( 必须是Json配置文件，由命令行参数指定 )
  命令行参数";
            sb.Append(localConfigInfo);
            sb.AppendLine();
            sb.Append("---------------------------------------------------");
            sb.AppendLine();
            sb.Append("参数信息，建议 ContentRoot 目录添加 help.info 文件，用于参数说明").AppendLine();
            sb.Append($"  {M.cr_arg} | {M.ContentRoot_Arg}  <dir>  指定内容根目录(ContentRoot)").AppendLine();
            sb.Append($"  {M.cf_arg} | {M.ConfigFile_Arg}  <path>  指定配置文件").AppendLine();

            string helpFile = Path.Combine(contentRoot, "help.info");
            if (File.Exists(helpFile))
            {
                string help = File.ReadAllText(helpFile, Encoding.UTF8);
                sb.Append("------------------ Help File ------------------");
                sb.AppendLine();
                sb.Append(help);
                sb.AppendLine();
                sb.Append("-----------------------------------------------");
            }

            sb.AppendLine();
            sb.AppendFormat("input args: {0}", string.Join(" ", args));
            sb.AppendLine();
            sb.AppendFormat($"{M.ContentRootKey}: \"{M.TEMP_CONFIG_DIC[M.ContentRootKey]}\"");
            sb.AppendLine();
            sb.AppendFormat($"{M.DefaultConfigFileKey}: \"{M.TEMP_CONFIG_DIC[M.DefaultConfigFileKey]}\"");
            sb.AppendLine();
            sb.AppendFormat($"{M.DefaultEnvConfigFileKey}: \"{M.TEMP_CONFIG_DIC[M.DefaultEnvConfigFileKey]}\"");
            sb.AppendLine();
            sb.AppendFormat($"{M.CustomConfigFileKey}: \"{M.TEMP_CONFIG_DIC[M.CustomConfigFileKey]}\"");
            sb.AppendLine();
            sb.AppendFormat($"{M.ArgsConfigFileKey}: \"{M.TEMP_CONFIG_DIC[M.ArgsConfigFileKey]}\"");
            sb.AppendLine();

            string msg = sb.ToString();
            using (var streamWriter = File.AppendText(file))
            {
                streamWriter.WriteLine(msg);
            }
            Console.WriteLine(msg);
        }
        /// <summary>
        /// 写入异常日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="file"></param>
        public static void WriteErrorLog(Exception ex, string file)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("=============================== Error ===============================");
            sb.AppendLine();
            sb.AppendFormat("{0:yyyy-MM-dd HH:mm:ss.ffff zzz} Program Error. Details:", DateTimeOffset.Now);
            sb.AppendLine();
            sb.Append(ex.ToString());
            sb.AppendLine();
            string msg = sb.ToString();
            using (var streamWriter = File.AppendText(file))
            {
                streamWriter.WriteLine(msg);
            }
            Console.WriteLine(msg);
        }
        /// <summary>
        /// 写入退出日志
        /// </summary>
        /// <param name="file"></param>
        public static void WriteExitLog(string file)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:yyyy-MM-dd HH:mm:ss.ffff zzz} Program Exited.", DateTimeOffset.Now);
            sb.AppendLine();
            string msg = sb.ToString();
            using (var streamWriter = File.AppendText(file))
            {
                streamWriter.WriteLine(msg);
            }
            Console.WriteLine(msg);
        }
    }
}
