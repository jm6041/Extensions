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
        /// <param name="endDir">结束的目录</param>
        /// <returns></returns>
        public static string? GetPathOfFileAbove(string searchPattern, string startDir, string? endDir = null)
        {
            DirectoryInfo dir = new DirectoryInfo(startDir);
            string? ffn;
            if (endDir != null && endDir.Length != 0)
            {
                DirectoryInfo endDirInfo = new DirectoryInfo(endDir);
                ffn = GetPathOfFileAbove(searchPattern, dir, endDirInfo);
            }
            else
            {
                ffn = GetPathOfFileAbove(searchPattern, dir, null);
            }
            return ffn;
        }
        /// <summary>
        /// 向上查找文件
        /// </summary>
        /// <param name="searchPattern">查询的文件名模式</param>
        /// <param name="startDir">开始的目录</param>
        /// <param name="endDir">结束的目录</param>
        /// <returns></returns>
        public static string? GetPathOfFileAbove(string searchPattern, DirectoryInfo startDir, DirectoryInfo? endDir = null)
        {
            if (!startDir.Exists)
            {
                throw new DirectoryNotFoundException(startDir.FullName);
            }
            if (endDir != null && endDir.Exists)
            {
                GetPathOfFileAboveInner2(new DirectoryInfo(startDir.FullName.TrimEnd('/', '\\')), new DirectoryInfo(endDir.FullName.TrimEnd('/', '\\')), ref searchPattern, out string? ffn);
                return ffn;
            }
            else
            {
                GetPathOfFileAboveInner1(new DirectoryInfo(startDir.FullName), ref searchPattern, out string? ffn);
                return ffn;
            }
        }
        /// <summary>
        /// 目录向上递归查找
        /// </summary>
        /// <param name="dir">递归查询的目录</param>
        /// <param name="fn">查询的文件名</param>
        /// <param name="ffn">递归查找到的文件全民</param>
        private static void GetPathOfFileAboveInner1(DirectoryInfo dir, ref string fn, out string? ffn)
        {
            FileInfo? file = dir.EnumerateFiles(fn, SearchOption.TopDirectoryOnly).OrderByDescending(x => x.LastWriteTimeUtc).FirstOrDefault();
            if (file != null)
            {
                ffn = file.FullName;
            }
            else
            {
                if (dir.Parent != null)
                {
                    GetPathOfFileAboveInner1(dir.Parent, ref fn, out ffn);
                }
                else
                {
                    ffn = null;
                }
            }
        }
        /// <summary>
        /// 目录向上递归查找
        /// </summary>
        /// <param name="dir">递归查询的目录</param>
        /// <param name="endDir">向上递归结束目录</param>
        /// <param name="fn">查询的文件名</param>
        /// <param name="ffn">递归查找到的文件全民</param>
        private static void GetPathOfFileAboveInner2(DirectoryInfo dir, DirectoryInfo endDir, ref string fn, out string? ffn)
        {
            FileInfo? file = dir.EnumerateFiles(fn, SearchOption.TopDirectoryOnly).OrderByDescending(x => x.LastWriteTimeUtc).FirstOrDefault();
            if (file != null)
            {
                ffn = file.FullName;
                return;
            }
            if (dir.FullName.Equals(endDir.FullName, StringComparison.OrdinalIgnoreCase))
            {
                ffn = null;
                return;
            }
            if (dir.Parent != null)
            {
                GetPathOfFileAboveInner2(dir.Parent, endDir, ref fn, out ffn);
            }
            else
            {
                ffn = null;
            }
        }
    }
}
