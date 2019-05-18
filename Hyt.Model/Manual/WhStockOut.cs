using System;
using System.Collections.Generic;

namespace Hyt.Model
{

    /// <summary>
    /// 出库单扩展属性
    /// </summary>
    /// <remarks>2013-06-08 周瑜 创建</remarks>
    public partial class WhStockOut : BaseEntity
    {

        /// <summary>
        /// 出库单中的所有商品
        /// </summary>
        public IList<WhStockOutItem> Items { get; set; } 

        /// <summary>
        /// 关联订单
        /// </summary>
        public SoOrder SoOrder { get; set; }
    }
}
