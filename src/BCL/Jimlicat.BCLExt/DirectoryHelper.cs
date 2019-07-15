using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO
{
    class DirectoryHelper
    {
        /// <summary>
        /// 向上查找文件
        /// </summary>
        /// <param name="fn">文件名</param>
        /// <param name="startDir">开始的目录</param>
        /// <returns></returns>
        public static string GetPathOfFileAbove(string fn, string startDir)
        {
            DirectoryInfo dir = new DirectoryInfo(startDir);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(startDir);
            }
            GetPathOfFileAboveInner(dir.Parent, ref fn, out string ffn);
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
                FileInfo file = dir.EnumerateFiles(fn, SearchOption.TopDirectoryOnly).FirstOrDefault();
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
