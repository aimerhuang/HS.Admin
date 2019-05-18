using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 商品属性
    /// </summary>
    /// <remarks>2014-01-13 ZTJ 添加注释</remarks>
    [Serializable]
    public class CBProductAttribute
    {
        /// <summary>
        /// 商品属性系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 属性名称(前台显示用)
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// 商品属性选项
        /// </summary>
        public IList<CBAttributeOption> AttributeOptions { get; set; }
    }

    /// <summary>
    /// 商品属性选项
    /// </summary>
    /// <remarks>2014-01-13 ZTJ 添加注释</remarks>
    [Serializable]
    public class CBAttributeOption
    {
        /// <summary>
        /// 商品属性选项编号
        /// </summary>
        public int AttributeOptionSysNo { get; set; }

        /// <summary>
        /// 产品编号(产品详情用)
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 属性文本值
        /// </summary>
        public string AttributeText { get; set; }

        /// <summary>
        /// 属性值图片
        /// </summary>
        public string Image { get; set; }
    }
}
