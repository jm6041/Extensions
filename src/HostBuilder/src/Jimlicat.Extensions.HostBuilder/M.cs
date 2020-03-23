using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// 公共字段常量
    /// </summary>
    internal class M
    {
        /// <summary>
        /// 启动状态文件配置Key
        /// </summary>
        public const string StartupStatusFileKey = "StartupStatusFile";

        /// <summary>
        /// 指定配置文件参数，简写
        /// </summary>
        public const string cf_arg = "-cf";
        /// <summary>
        /// 指定配置文件参数
        /// </summary>
        public const string ConfigFile_Arg = "--ConfigFile";
        /// <summary>
        /// 默认配置文件Key
        /// </summary>
        public const string DefaultConfigFileKey = "DefaultConfigFile";
        /// <summary>
        /// 默认环境配置文件Key
        /// </summary>
        public const string DefaultEnvConfigFileKey = "DefaultEnvConfigFile";
        /// <summary>
        /// 自定义配置文件Key
        /// </summary>
        public const string CustomConfigFileKey = "CustomConfigFile";
        /// <summary>
        /// 参数自定配置文件Key
        /// </summary>
        public const string ArgsConfigFileKey = "ArgsConfigFile";
        /// <summary>
        /// 临时配置字典
        /// </summary>
        public static readonly Dictionary<string, string> TEMP_CONFIG_DIC = new Dictionary<string, string>()
        {
            {DefaultConfigFileKey, ""},
            {DefaultEnvConfigFileKey, ""},
            {CustomConfigFileKey, ""},
            {ArgsConfigFileKey, "" },
        };
    }
}
