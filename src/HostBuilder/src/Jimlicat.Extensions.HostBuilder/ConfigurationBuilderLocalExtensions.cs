using System;
using System.IO;
using System.Linq;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// 本地配置扩展
    /// </summary>
    public static class ConfigurationBuilderLocalExtensions
    {
        /// <summary>
        /// 添加默认配置文件
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="contentRoot"></param>
        /// <param name="environmentName"></param>
        /// <param name="endDir">递归向上查找文件的结束目录</param>
        public static void AddDefaultConfigFile(this IConfigurationBuilder configurationBuilder, string contentRoot, string environmentName, DirectoryInfo? endDir)
        {
            // 默认配置文件
            string dconfig = $"default.json";
            DirectoryInfo dir = new DirectoryInfo(contentRoot);
            if (dir.Parent == null)
            {
                return;
            }
            var defaultFile = DirectoryHelper.GetPathOfFileAbove(dconfig, dir.Parent, endDir);
            if (defaultFile != null && defaultFile.Length != 0)
            {
                configurationBuilder.AddJsonFile(defaultFile, true, true);
                M.TEMP_CONFIG_DIC[M.DefaultConfigFileKey] = defaultFile;
            }

            // 运行环境的默认配置文件
            var defaultEnvFile = DirectoryHelper.GetPathOfFileAbove($"default.{environmentName}.json", dir.Parent, endDir);
            if (defaultEnvFile != null && defaultEnvFile.Length != 0)
            {
                configurationBuilder.AddJsonFile(defaultEnvFile, true, true);
                M.TEMP_CONFIG_DIC[M.DefaultEnvConfigFileKey] = defaultEnvFile;
            }
        }

        /// <summary>
        /// 添加文件夹配置文件
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="dir"></param>
        public static void AddDirFiles(this IConfigurationBuilder configurationBuilder, string dir)
        {
            if (Directory.Exists(dir))
            {
                DirectoryInfo d = new DirectoryInfo(dir);
                var jsonFiles = d.EnumerateFiles("*.json", SearchOption.AllDirectories).OrderBy(x => x.Name).ToArray();
                foreach (var jsonFile in jsonFiles)
                {
                    configurationBuilder.AddJsonFile(jsonFile.FullName, true, true);
                }
                var iniFiles = d.EnumerateFiles("*.ini", SearchOption.AllDirectories).OrderBy(x => x.Name).ToArray();
                foreach (var iniFile in iniFiles)
                {
                    configurationBuilder.AddIniFile(iniFile.FullName, true, true);
                }
            }
        }

        /// <summary>
        /// secrets
        /// </summary>
        public const string secrets = "secrets";
        /// <summary>
        /// config
        /// </summary>
        public const string config = "config";
        /// <summary>
        /// 添加本地配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="contentRoot"></param>
        /// <param name="environmentName"></param>
        /// <param name="customConfigJsonFile">自定义配置目录</param>
        /// <param name="args">启动参数</param>
        /// <param name="argsConfigJsonFile">参数指定配置目录</param>
        /// <param name="endDir">递归向上查找文件的结束目录</param>
        public static IConfigurationBuilder AddLocalConfiguration(this IConfigurationBuilder builder, string contentRoot, string environmentName, string customConfigJsonFile, string[] args, string argsConfigJsonFile, DirectoryInfo? endDir)
        {
            // 默认配置文件
            builder.AddDefaultConfigFile(contentRoot, environmentName, endDir);

            // config配置
            builder.AddJsonFile("config.json", true, true);
            builder.AddIniFile("config.ini", true, true);

            builder.AddJsonFile($"config.{environmentName}.json", true, true);
            builder.AddIniFile($"config.{environmentName}.ini", true, true);

            builder.AddJsonFile($"config.{Environment.MachineName}.json", true, true);
            builder.AddIniFile($"config.{Environment.MachineName}.ini", true, true);

            builder.AddJsonFile($"config.{Environment.UserName}.json", true, true);
            builder.AddIniFile($"config.{Environment.UserName}.ini", true, true);

            // 添加 secrets 目录配置文件
            builder.AddDirFiles(Path.Combine(contentRoot, secrets));

            // 添加 config 目录配置文件
            builder.AddDirFiles(Path.Combine(contentRoot, config));

            // 自定义配置文件
            if (!string.IsNullOrEmpty(customConfigJsonFile))
            {
                builder.AddJsonFile(customConfigJsonFile, true, true);
            }
            if (args != null)
            {
                // 参数指定配置文件
                if (!string.IsNullOrEmpty(argsConfigJsonFile))
                {
                    builder.AddJsonFile(argsConfigJsonFile, true, true);
                }
                builder.AddCommandLine(args);
            }
            return builder;
        }
    }
}
