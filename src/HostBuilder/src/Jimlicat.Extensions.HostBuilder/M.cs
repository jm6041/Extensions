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
        /// 指定 ContentRoot 参数，简写
        /// </summary>
        public const string cr_arg = "-cr";
        /// <summary>
        /// 指定 ContentRoot 参数
        /// </summary>
        public const string ContentRoot_Arg = "--ContentRoot";
        /// <summary>
        /// ContentRoot Key
        /// </summary>
        public const string ContentRootKey = "ContentRoot";
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
        /// 参数指定配置文件Key
        /// </summary>
        public const string ArgsConfigFileKey = "ArgsConfigFile";
        /// <summary>
        /// 临时配置字典
        /// </summary>
        public static readonly Dictionary<string, string> TEMP_CONFIG_DIC = new Dictionary<string, string>()
        {
            {ContentRootKey, "" },
            {DefaultConfigFileKey, ""},
            {DefaultEnvConfigFileKey, ""},
            {CustomConfigFileKey, ""},
            {ArgsConfigFileKey, "" },
        };
    }
}
