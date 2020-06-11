/*using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 名字类型
    /// </summary>
    public enum NameType
    {
        /// <summary>
        /// 正常，表名与实体类名一样，字段名与实体类属性名一样，适用于 mssql
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 小写，用下划线'_'隔开，适用于 mysql, postgresql
        /// </summary>
        Lower = 1,
        /// <summary>
        /// 大写，用下划线'_'隔开，适用于 oracle
        /// </summary>
        Upper = 2,
    }
    /// <summary>
    /// <see cref="NameType"/> 帮助类
    /// </summary>
    public static class NameTypeHelper
    {
        /// <summary>
        /// 获得正确的名字
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nt"></param>
        /// <returns></returns>
        public static string GetName(string name, NameType nt)
        {
            switch (nt)
            {
                case NameType.Normal:
                    return name;
                case NameType.Lower:
                    return GetLower(name);
                case NameType.Upper:
                    return GetUpper(name);
                default:
                    return name;
            }
        }
        /// <summary>
        /// 获得小写
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetLower(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            List<char> nl = new List<char>(name.Length + 10)
            {
                char.ToLower(name[0])
            };
            for (int i = 1; i < name.Length; i++)
            {
                char ci = name[i];
                if (char.IsUpper(ci))
                {
                    if (char.IsUpper(name[i - 1]))
                    {
                        nl.Add(char.ToLower(ci));
                    }
                    else
                    {
                        nl.Add('_');
                        nl.Add(char.ToLower(ci));
                    }
                }
                else
                {
                    nl.Add(ci);
                }
            }
            return new string(nl.ToArray());
        }
        /// <summary>
        /// 获得大写
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetUpper(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            List<char> nl = new List<char>(name.Length + 10)
            {
                char.ToUpper(name[0])
            };
            for (int i = 1; i < name.Length; i++)
            {
                char ci = name[i];
                if (char.IsLower(ci))
                {
                    if (char.IsLower(name[i - 1]))
                    {
                        nl.Add(char.ToUpper(ci));
                    }
                    else
                    {
                        nl.Add('_');
                        nl.Add(char.ToUpper(ci));
                    }
                }
                else
                {
                    nl.Add(ci);
                }
            }
            return new string(nl.ToArray());
        }
    }
}
*/