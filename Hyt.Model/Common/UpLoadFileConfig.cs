using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 上传文件配置
    /// </summary>
    /// <remarks>2013-12-06 黄波 创建</remarks>
    [Serializable]
    public class UpLoadFileConfig : ConfigBase
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        public FileConfigOption DefaultConfig { get; set; }

        /// <summary>
        /// 其他配置
        /// </summary>
        public List<FileConfigOption> OtherConfig { get; set; }
    }

    /// <summary>
    /// 上传文件配置选项
    /// </summary>
    /// <remarks>2013-12-06 黄波 创建</remarks>
    [Serializable]
    public class FileConfigOption
    {
        /// <summary>
        /// 加密的别名,用于在客户端传递
        /// </summary>
        public string EncryptAlias { get; set; }

        /// <summary>
        /// 文件最大大小
        /// </summary>
        public string MaxSize { get; set; }

        /// <summary>
        /// 文件类型 *.jpg;*.png  或者 全部文件 *.*
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 文件类型说明  图片文件 或者 全部文件
        /// </summary>
        public string TypeDescript { get; set; }

        /// <summary>
        /// 允许同时上传的文件数量
        /// </summary>
        public int QueueLimit { get; set; }

        /// <summary>
        /// 保存文件的文件夹名称
        /// </summary>
        public string Folder { get; set; }
    }
}
