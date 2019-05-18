using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.DouShabaoModel
{
    /// <summary>
    /// 豆沙包订单明细（海淘）
    /// </summary>
    /// <remarks>2017-7-07 罗熙 创建</remarks>
    public class ProductList
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品类别
        /// </summary>
        public string ProductCategory { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductNum { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public string ProductPrice { get; set; }
        /// <summary>
        /// 商品品牌
        /// </summary>
        public string ProductBrand { get; set; }
        /// <summary>
        /// 目的地城市
        /// </summary>
        public string DestinationCity { get; set; }
    }
}
