using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Lib.Log;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;
using Serilog;

#if NETCOREAPP3_1
namespace Microsoft.Extensions.Hosting
#else
namespace Microsoft.AspNetCore.Hosting
#endif
{
    /// <summary>
    /// 与系统启动相关函数
    /// </summary>
    public static class P
    {
        /// <summary>
        /// 是否Windows服务托管
        /// </summary>
        private static readonly bool IS_WINDOWS_SERVICE;
        /// <summary>
        /// ContentRoot
        /// </summary>
        private static readonly string CONTENT_ROOT;
        /// <summary>
        /// 自定义JSON配置文件
        /// </summary>
        private static readonly string CUSTOM_CONFIG_JSONFILE;        
        /// <summary>
        /// ASPNETCORE_ENVIRONMENT 指定的环境名
        /// </summary>
        private static readonly string ENVIRONMENT;
        /// <summary>
        /// CTC_CLUSTER
        /// </summary>
        private static readonly string CTC_CLUSTER;
        /// <summary>
        /// CTC_CONSUL_HOST
        /// </summary>
        private static readonly string CTC_CONSUL_HOST = "localhost";
        /// <summary>
        /// CTC_CONSUL_PORT
        /// </summary>
        private static readonly string CTC_CONSUL_PORT = "80";
        /// <summary>
        /// Consule Uri
        /// </summary>
        private static readonly string CONSUL_URI = "";
        /// <summary>
        /// 是否为K8S环境，如果K8S环境，从CONSUL读取配置
        /// </summary>
        public static readonly bool IS_K8S;
        /// <summary>
        /// 启动 ConfigurationBuilder，读取
        /// appsettings.json
        /// appsettings.{EnvironmentName}.json
        /// 环境变量
        /// 扩展的配置，扩展默认文件，config配置，secrets 目录配置文件，config 目录配置文件, 自定义配置文件
        /// </summary>
        public static ConfigurationBuilder StartupConfigurationBuilder { get; }
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static P()
        {
            IS_WINDOWS_SERVICE = WindowsServiceHelpers.IsWindowsService();
            CONTENT_ROOT = GetContentRootInner(IS_WINDOWS_SERVICE);
            CUSTOM_CONFIG_JSONFILE = GetCustomConfigJsonFile(CONTENT_ROOT);
            ENVIRONMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            CTC_CLUSTER = Environment.GetEnvironmentVariable("CTC_CLUSTER");
            CTC_CONSUL_HOST = Environment.GetEnvironmentVariable("CTC_CONSUL_HOST");
            CTC_CONSUL_PORT = Environment.GetEnvironmentVariable("CTC_CONSUL_PORT");
            CONSUL_URI = $"http://{CTC_CONSUL_HOST}:{CTC_CONSUL_PORT}";
            IS_K8S = CTC_CLUSTER != null && CTC_CLUSTER.Equals("k8s", StringComparison.OrdinalIgnoreCase);
            M.TEMP_CONFIG_DIC[M.CustomConfigFileKey] = CUSTOM_CONFIG_JSONFILE;
            StartupConfigurationBuilder = CreateBuilder(CONTENT_ROOT, ENVIRONMENT, CUSTOM_CONFIG_JSONFILE);
        }

        private static ConfigurationBuilder CreateBuilder(string contentRoot, string environmentName, string customConfigJsonFile)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(contentRoot)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddextendedConfiguration(contentRoot, environmentName, customConfigJsonFile);
            return builder;
        }

        /// <summary>
        /// 获得 ContentRoot
        /// </summary>
        /// <param name="isws">是否Windows服务托管</param>
        /// <returns></returns>
        private static string GetContentRootInner(bool isws)
        {
            // 内容根目录，默认当前程序真实目录
            string contentRoot = AppContext.BaseDirectory;
            if (isws)
            {
                return contentRoot;
            }

            // 当前目录，
            string curDir = Directory.GetCurrentDirectory();
            // 当前目录信息
            DirectoryInfo curDirInfo = new DirectoryInfo(curDir);
            // 当前目录包含项目文件，表示从项目启动，例如 dotnet run， 必须使用当前目录
            if (curDirInfo.EnumerateFiles("*.csproj").Any())
            {
                contentRoot = curDir;
            }
            return contentRoot;
        }

        /// <summary>
        /// 前进程是否作为Windows服务托管
        /// </summary>
        /// <returns></returns>
        public static bool IsWindowsService()
        {
            return IS_WINDOWS_SERVICE;
        }

        /// <summary>
        /// 获得 ContentRoot
        /// </summary>
        /// <returns></returns>
        public static string GetContentRoot()
        {
            return CONTENT_ROOT;
        }
        /// <summary>
        /// 获得自定义配置文件
        /// </summary>
        /// <returns></returns>
        private static string GetCustomConfigJsonFile(string contentRoot)
        {
            DirectoryInfo dir = new DirectoryInfo(contentRoot);
            string file = DirectoryHelper.GetPathOfFileAbove("*config.json", dir.Parent);
            // 自定义配置文件不存在，用空文件代替
            if (string.IsNullOrEmpty(file))
            {
                file = "";
            }
            return file;
        }
        /// <summary>
        /// HostBuilder
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="startupStatusFile">启动状态保存文件</param>
        /// <param name="sharedKey">consul共享Key</param>
        /// <param name="specialKey">consul特定Key</param>
        /// <returns></returns>
#if NETCOREAPP2_1 || NET461
        public static IWebHostBuilder CreateHostBuilder(string[] args, string startupStatusFile, string sharedKey = null, string specialKey = null)
        {
            string contentRoot = CONTENT_ROOT;
            // 参数指定配置文件
            string argsConfigJsonFile = GetArgsConfigJsonFile(args);
            M.TEMP_CONFIG_DIC[M.ArgsConfigFileKey] = argsConfigJsonFile;

            var hostBuilder = WebHost.CreateDefaultBuilder(args);
            if (!string.IsNullOrWhiteSpace(startupStatusFile))
            {
                hostBuilder.ConfigureAppConfiguration((config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string>() {
                        { M.StartupStatusFileKey, startupStatusFile }
                    });
                });
            }
            hostBuilder.UseContentRoot(contentRoot)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    if (IS_K8S)
                    {
                        builder.AddConsulConfiguration(CONSUL_URI, sharedKey, specialKey, args);
                    }
                    else
                    {
                        var env = context.HostingEnvironment;
                        builder.AddLocalConfiguration(contentRoot, env.EnvironmentName, CUSTOM_CONFIG_JSONFILE, args, argsConfigJsonFile);
                    }
                })
                .ConfigureLogging((hostContext, builder) =>
                {
                    if (IS_K8S)
                    {
                        builder.ClearProviders();
                        NLogOptions nLogOptions = null;
                        KafkaOptions kafkaOptions = null;
                        if (hostContext.Configuration.GetSection("Logging:Local:Enable").Get<bool>())
                        {
                            Console.WriteLine("Enable local logging");
                            nLogOptions = new NLogOptions
                            {
                                NLogSavePath = "log"
                            };
                        }
                        if (hostContext.Configuration.GetSection("Logging:Mixed:Enable").Get<bool>())
                        {
                            Console.WriteLine("Enable Kafka logging");
                            kafkaOptions = new KafkaOptions
                            {
                                BrokerList = hostContext.Configuration["Logging:Mixed:BrokerList"],
                                TopicName = hostContext.Configuration["Logging:Mixed:TopicName"],
                                Project = hostContext.Configuration["Logging:Mixed:Project"],
                                Environment = hostContext.Configuration["Logging:Mixed:Environment"],
                                ClientTag = hostContext.Configuration["Logging:Mixed:ClientTag"]
                            };
                        }
                        builder.Services.AddSingleton<ITraceContext, TraceContext>();
                        builder.AddMix(kafkaOptions, nLogOptions, hostContext.HostingEnvironment.IsDevelopment());
                    }
                    else
                    {
                        var logger = CreateLogger(hostContext.Configuration);
                        builder.AddSerilog(logger);
                    }
                });
            return hostBuilder;
        }
#else
        public static IHostBuilder CreateHostBuilder(string[] args, string startupStatusFile, string sharedKey = null, string specialKey = null)
        {
            string contentRoot = CONTENT_ROOT;
            // 参数指定配置文件
            string argsConfigJsonFile = GetArgsConfigJsonFile(args);
            M.TEMP_CONFIG_DIC[M.ArgsConfigFileKey] = argsConfigJsonFile;

            var hostBuilder = Host.CreateDefaultBuilder(args);
            if (!string.IsNullOrWhiteSpace(startupStatusFile))
            {
                hostBuilder.ConfigureHostConfiguration((config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string>() {
                        { M.StartupStatusFileKey, startupStatusFile }
                    });
                });
            }
            hostBuilder.UseContentRoot(contentRoot)
                .UseWindowsService()
                .UseSystemd()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    if (IS_K8S)
                    {
                        builder.AddConsulConfiguration(sharedKey, specialKey, CONSUL_URI, args);
                    }
                    else
                    {
                        var env = context.HostingEnvironment;
                        builder.AddLocalConfiguration(contentRoot, env.EnvironmentName, CUSTOM_CONFIG_JSONFILE, args, argsConfigJsonFile);
                    }
                })
                .ConfigureLogging((hostContext, builder) =>
                {
                    if (IS_K8S)
                    {
                        builder.ClearProviders();
                        NLogOptions nLogOptions = null;
                        KafkaOptions kafkaOptions = null;
                        if (hostContext.Configuration.GetSection("Logging:Local:Enable").Get<bool>())
                        {
                            Console.WriteLine("Enable local logging");
                            nLogOptions = new NLogOptions
                            {
                                NLogSavePath = "log"
                            };
                        }
                        if (hostContext.Configuration.GetSection("Logging:Mixed:Enable").Get<bool>())
                        {
                            Console.WriteLine("Enable Kafka logging");
                            kafkaOptions = new KafkaOptions
                            {
                                BrokerList = hostContext.Configuration["Logging:Mixed:BrokerList"],
                                TopicName = hostContext.Configuration["Logging:Mixed:TopicName"],
                                Project = hostContext.Configuration["Logging:Mixed:Project"],
                                Environment = hostContext.Configuration["Logging:Mixed:Environment"],
                                ClientTag = hostContext.Configuration["Logging:Mixed:ClientTag"]
                            };
                        }
                        builder.Services.AddSingleton<ITraceContext, TraceContext>();
                        builder.AddMix(kafkaOptions, nLogOptions, hostContext.HostingEnvironment.IsDevelopment());
                    }
                    else
                    {
                        var logger = CreateLogger(hostContext.Configuration);
                        builder.AddSerilog(logger);
                    }
                });
            return hostBuilder;
        }
#endif

        private static Serilog.ILogger CreateLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        /// <summary>
        /// 参数获得指定配置文件
        /// </summary>
        /// <param name="args">启动参数</param>
        /// <returns></returns>
        private static string GetArgsConfigJsonFile(string[] args)
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

        /// <summary>
        /// 获得指定前缀的文件名
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="prefix"></param>
        /// <param name="count">保留前缀的文件数量，默认为3</param>
        /// <returns></returns>
        public static string GetFileName(string dir, string prefix, int count = 3)
        {
            // 入口程序集名
            var entryName = Assembly.GetEntryAssembly().GetName().Name;
            var start = entryName + "-" + prefix;
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            // 删除早期的文件
            var dfs = dirInfo.EnumerateFiles(start + "-*.log").OrderByDescending(x => x.LastWriteTimeUtc).Skip(count).ToList();
            foreach (var df in dfs)
            {
                df.Delete();
            }
            DateTimeOffset now = DateTimeOffset.Now;
            string n = string.Format("{0}-{1:yyyyMMdd}-{2}.log", start, now, now.Ticks);
            string fn = Path.Combine(dir, n);
            return fn;
        }
        /// <summary>
        /// 写入启动日志
        /// </summary>
        /// <param name="args">启动参数</param>
        /// <param name="file">写入的文件</param>
        /// <param name="sharedKey">consul共享Key</param>
        /// <param name="specialKey">consul特定Key</param>
        public static void WriteStartupLog(string[] args, string file, string sharedKey = null, string specialKey = null)
        {
            // 是否迁移数据库
            StringBuilder sb = new StringBuilder();
            // 入口程序集名
            var entryName = Assembly.GetEntryAssembly().GetName().Name;
            sb.AppendFormat("{0:yyyy-MM-dd HH:mm:ss.ffff zzz} Program {1} Starting", DateTimeOffset.Now, entryName);
            sb.AppendLine();
#if DEBUG
            sb.AppendLine("DEBUG");
#endif
#if RELEASE
            sb.AppendLine("RELEASE");
#endif
            string contentRoot = CONTENT_ROOT;
            sb.AppendFormat($"ContentRoot: \"{contentRoot}\"");
            sb.AppendLine();
            sb.AppendLine();
            if (IS_K8S)
            {
                sb.Append($"CTC_CLUSTER:{CTC_CLUSTER}");
                sb.AppendLine();
                sb.Append($"  CTC_CONSUL_HOST:{CTC_CONSUL_HOST}");
                sb.AppendLine();
                sb.Append($"  CTC_CONSUL_PORT:{CTC_CONSUL_PORT}");
                sb.AppendLine();
                sb.Append($"  CONSUL_URI:{CONSUL_URI}");
                sb.AppendLine();
                sb.Append($"    {sharedKey}");
                sb.AppendLine();
                sb.Append($"    {specialKey}");
                sb.AppendLine();
            }
            else
            {
                sb.Append("=============================== 配置说明 ===============================");
                sb.AppendLine();
                string localConfigInfo = @"所有的配置都是键值对，在环境变量中使用双下划线'__'代替英文冒号':'，在参数中格式为'--Key=Value'
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
  指定配置文件 ( 必须是Json配置文件，由命令行参数指定 )
  命令行参数";
                sb.Append(localConfigInfo);
                sb.AppendLine();
                sb.Append("=========================================================================");
                sb.AppendLine();
                sb.AppendLine();
                sb.Append("参数信息，建议 ContentRoot 目录添加 help.info 文件，用于参数说明").AppendLine();
                sb.Append($"  {M.cf_arg} | {M.ConfigFile_Arg}  <path>  指定配置文件").AppendLine();


                string helpFile = Path.Combine(contentRoot, "help.info");
                if (File.Exists(helpFile))
                {
                    string help = File.ReadAllText(helpFile, Encoding.UTF8);
                    sb.Append("=============================== Help File ===============================");
                    sb.AppendLine();
                    sb.Append(help);
                    sb.AppendLine();
                    sb.Append("=========================================================================");
                }

                sb.AppendLine();
                sb.AppendLine();
                sb.AppendFormat("input args: {0}", string.Join(" ", args));
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendFormat($"{M.DefaultConfigFileKey}: \"{M.TEMP_CONFIG_DIC[M.DefaultConfigFileKey]}\"");
                sb.AppendLine();
                sb.AppendFormat($"{M.DefaultEnvConfigFileKey}: \"{M.TEMP_CONFIG_DIC[M.DefaultEnvConfigFileKey]}\"");
                sb.AppendLine();
                sb.AppendFormat($"{M.CustomConfigFileKey}: \"{M.TEMP_CONFIG_DIC[M.CustomConfigFileKey]}\"");
                sb.AppendLine();
                sb.AppendFormat($"{M.ArgsConfigFileKey}: \"{M.TEMP_CONFIG_DIC[M.ArgsConfigFileKey]}\"");
                sb.AppendLine();
            }
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
            string msg = string.Format("{0:yyyy-MM-dd HH:mm:ss.ffff zzz} Program Error: {1} ", DateTimeOffset.Now, ex);
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
            string msg = string.Format("{0:yyyy-MM-dd HH:mm:ss.ffff zzz} Program Exited. ContentRoot: \"{1}\" ", DateTimeOffset.Now, CONTENT_ROOT);
            using (var streamWriter = File.AppendText(file))
            {
                streamWriter.WriteLine(msg);
            }
            Console.WriteLine(msg);
        }
    }
}
