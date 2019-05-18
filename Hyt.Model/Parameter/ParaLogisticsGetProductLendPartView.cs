using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 用于传递补单页面商品数据
    /// </summary>
    /// <remarks>2013-09-26 沈强 创建</remarks>
    [Serializable]
    public class ParaLogisticsGetProductLendPartView
    {
        /// <summary>
        /// 客户系统编号
        /// </summary>
        /// <remarks>2013-09-26 沈强 创建</remarks>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 购物车缓存key
        /// </summary>
        /// <remarks>2013-09-26 沈强 创建</remarks>
        public string KeyCache { get; set; }

        /// <summary>
        /// 弹出框选择的商品集合
        /// </summary>
        /// <remarks>2013-09-26 沈强 创建</remarks>
        public List<Product> Products { get; set; }
    }
    /// <summary>
    /// 补单商品数据
    /// </summary>
    /// <remarks>2013-09-26 沈强 创建</remarks>
    public class Product
    {
        /// <summary>
        /// 类型[product=>商品; group=>组合、团购、促销]
        /// </summary>
        /// <remarks>2013-09-26 沈强 创建</remarks>
        public string Type { get; set; }

        /// <summary>
        /// 商品系统编号
        /// </summary>
        /// <remarks>2013-09-26 沈强 创建</remarks>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 组合,团购主表系统编号
        /// </summary>
        /// <remarks>2013-09-26 沈强 创建</remarks>
        public int GroupSysNo { get; set; }

        /// <summary>
        /// 促销系统编号
        /// </summary>
        /// <remarks>2013-09-26 沈强 创建</remarks>
        public int PromotionSysNo { get; set; }
    }
}
