using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 商品关联
    /// </summary>
    /// <remarks>2013-07-23 邵斌 创建</remarks>
    [Serializable]
    public class CBProductAssociation
    {
        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string ERPCode { get; set; }

        /// <summary>
        /// 商品属性
        /// </summary>
        public int AttributeSysNo { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// 商品属性文本值
        /// </summary>
        public string AttributeText { get; set; }

        /// <summary>
        /// 商品属性图片值
        /// </summary>
        public string AttributeImage { get; set; }

        /// <summary>
        /// 商品属性选项系统编号
        /// </summary>
        public int AttributeOptionSysNo { get; set; }

        /// <summary>
        /// 前台显示
        /// </summary>
        public int IsFrontDisplay { get; set; }
    }
}
