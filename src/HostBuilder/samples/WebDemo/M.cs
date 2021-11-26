using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDemo
{
    internal class M
    {
        /// <summary>
        /// AllowCors
        /// </summary>
        public const string AllowCors = "AllowCors";
        public const string ApiScope = "api";
        /// <summary>
        /// 应用名
        /// </summary>
        internal static readonly string? AppName = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name;
    }
}
