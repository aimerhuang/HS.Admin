using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 订单标识配置
    /// </summary>
    /// <remarks>2014-05-19 余勇 创建</remarks>
    [Serializable]
    public class OrderImgFlagConfig : ConfigBase
    {
        /// <summary>
        /// 订单标识配置
        /// </summary>
        public List<ImgConfigOption> ImgConfig { get; set; }
    }

    /// <summary>
    /// 订单标识配置选项
    /// </summary>
    /// <remarks>2014-05-19 余勇 创建</remarks>
    [Serializable]
    public class ImgConfigOption
    {
        /// <summary>
        /// 标识名称
        /// </summary>
        public string ImgFlag { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 标识说明
        /// </summary>
        public string Descript { get; set; }
    }
}
