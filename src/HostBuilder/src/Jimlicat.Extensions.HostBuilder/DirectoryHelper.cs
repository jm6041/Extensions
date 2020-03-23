using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO
{
    /// <summary>
    /// 目录帮助方法
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// 向上查找文件
        /// </summary>
        /// <param name="searchPattern">查询的文件名模式"/></param>
        /// <param name="startDir">开始的目录</param>
        /// <returns></returns>
        public static string GetPathOfFileAbove(string searchPattern, string startDir)
        {
            DirectoryInfo dir = new DirectoryInfo(startDir);
            string ffn = GetPathOfFileAbove(searchPattern, dir);
            return ffn;
        }

        /// <summary>
        /// 向上查找文件
        /// </summary>
        /// <param name="searchPattern">查询的文件名模式</param>
        /// <param name="startDir">开始的目录</param>
        /// <returns></returns>
        public static string GetPathOfFileAbove(string searchPattern, DirectoryInfo startDir)
        {
            if (!startDir.Exists)
            {
                throw new DirectoryNotFoundException(startDir.FullName);
            }
            GetPathOfFileAboveInner(startDir, ref searchPattern, out string ffn);
            return ffn;
        }

        /// <summary>
        /// 目录向上递归查找
        /// </summary>
        /// <param name="dir">递归查询的目录</param>
        /// <param name="fn">查询的文件名</param>
        /// <param name="ffn">递归查找到的文件全民</param>
        private static void GetPathOfFileAboveInner(DirectoryInfo dir, ref string fn, out string ffn)
        {
            if (dir == null)
            {
                ffn = null;
            }
            else
            {
                FileInfo file = dir.EnumerateFiles(fn, SearchOption.TopDirectoryOnly).OrderByDescending(x => x.LastWriteTimeUtc).FirstOrDefault();
                if (file != null)
                {
                    ffn = file.FullName;
                }
                else
                {
                    GetPathOfFileAboveInner(dir.Parent, ref fn, out ffn);
                }
            }
        }
    }
}
