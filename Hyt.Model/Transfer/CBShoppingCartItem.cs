using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 购物车项传送
    /// </summary>
    /// <remarks>2014-01-13 ZTJ 添加注释</remarks>
    [Serializable]
    public class CBShoppingCartItem
    {
        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 商品购买数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
