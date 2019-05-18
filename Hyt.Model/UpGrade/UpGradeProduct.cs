using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.UpGrade
{
    /// <summary>
    /// 商品实体
    /// </summary>
    /// <remarks>2013-8-28 陶辉 创建</remarks>
    public class UpGradeProduct
    {
        /// <summary>
        /// 商城商品编码
        /// </summary>
        public int HytProductSysNo { get; set; }
        /// <summary>
        /// 商城商品ERP编码
        /// </summary>
        public string HytProductCode { get; set; }

        /// <summary>
        /// 商城商品名称
        /// </summary>
        public string HytProductName { get; set; }

        /// <summary>
        /// 第三方商品编码
        /// </summary>
        public string MallProductCode { get; set; }

        /// <summary>
        /// 第三方商品属性,多属性用英文半角分号隔开
        /// </summary>
        public string MallProductAttrs { get; set; }

        /// <summary>
        /// 第三方商品名称
        /// </summary>
        public string MallProductName { get; set; }

        /// <summary>
        /// 商城分销商商品价格
        /// </summary>
        public decimal HytPrice { get; set; }
    }
}
