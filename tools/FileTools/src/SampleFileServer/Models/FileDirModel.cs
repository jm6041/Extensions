using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleFileServer.Models
{
    /// <summary>
    /// 文件目录模型
    /// </summary>
    public class FileDirModel
    {
        /// <summary>
        /// 默认
        /// </summary>
        public static readonly FileDirModel Default = new FileDirModel()
        {
            Path = "/files",
            Dir = "/sources",
        };
        /// <summary>
        /// 文件请求路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 物理路径
        /// </summary>
        public string Dir { get; set; }
    }

    /// <summary>
    /// 文件目录信息
    /// </summary>
    public class FileDirsInfo
    {
        /// <summary>
        /// 目录配置信息
        /// </summary>
        public List<FileDirModel> Data { get; set; }
    }
}
