using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 物流信息扩展
    /// </summary>
    /// <remarks>
    /// 2014-05-20 朱成果 创建
    /// </remarks>
    public class CBLgExpressDetail : LgExpressInfo
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 出库单号
        /// </summary>
        public int OutStockNo { get; set; }
    }
}
