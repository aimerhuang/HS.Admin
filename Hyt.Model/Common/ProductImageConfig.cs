using Hyt.Model.Common;
using System;
using System.Collections.Generic;
namespace Hyt.Model
{
    /// <summary>
    /// 产品图片相关
    /// </summary>
    /// <remarks>2014-1-20 黄波 创建</remarks>
    [Serializable()]
    public class ProductImageConfig : ConfigBase
    {
        /// <summary>
        /// 图片地址规则
        /// {0} : 规格文件夹
        /// {1} : 年月编号
        /// {2} : 文件名称
        /// </summary>
        public string ProductImagePathFormat { get; set; }

        /// <summary>
        /// 原图存储的文件夹名称
        /// 上传的图片,未做任何更改的
        /// </summary>
        public string BaseFolder { get; set; }

        /// <summary>
        /// 小图文件夹名称
        /// </summary>
        public string SmallFloder { get; set; }

        /// <summary>
        /// 小图宽度
        /// </summary>
        public int SmallWidth { get; set; }

        /// <summary>
        /// 小图高度
        /// </summary>
        public int SmallHeight { get; set; }

        /// <summary>
        /// 大图文件夹名称
        /// </summary>
        public string BigFloder { get; set; }

        /// <summary>
        /// 大图宽度
        /// </summary>
        public int BigWidth { get; set; }

        /// <summary>
        /// 大图高度
        /// </summary>
        public int BigHeight { get; set; }

        /// <summary>
        /// 缩略图相关配置
        /// </summary>
        public List<ProductThumbnailConfig> Thumbnail { get; set; }
    }
    /// <summary>
    /// 缩略图配置
    /// </summary>
    /// <remarks>2014-1-20 黄波 创建</remarks>
    [Serializable()]
    public class ProductThumbnailConfig
    {
        /// <summary>
        /// 文件夹名称
        /// </summary>
        public string Folder { get; set; }
        /// <summary>
        /// 缩略图高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 缩略图宽度
        /// </summary>
        public int Width { get; set; }
    }
}