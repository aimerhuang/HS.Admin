using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 门店提货单
    /// </summary>
    /// <remarks>2013-07-06 朱家宏 创建</remarks>
    public class CBOutStockOrder
    {
        /// <summary>
        /// 出库单号
        /// </summary>
        public int StockOutSysNo { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 仓库名
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 收货人手机
        /// </summary>
        public string ReceiverMobile { get; set; }

        /// <summary>
        /// 订单人帐号
        /// </summary>
        public string CustomerAccount { get; set; }

        /// <summary>
        /// 订单人sysNo
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 应收款
        /// </summary>
        public decimal Receivable { get; set; }

        /// <summary>
        /// 出库单状态
        /// </summary>
        public int StockOutStatus { get; set; }

        /// <summary>
        /// 订单创建日期
        /// </summary>
        public DateTime OrderCreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}
