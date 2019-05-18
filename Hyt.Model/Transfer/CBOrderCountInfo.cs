using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 销售统计信息
    /// </summary>
    /// <remarks>2013-09-26 邵斌 创建</remarks>
    [Serializable]
    public class CBDefaultPageCountInfo
    {
        /// <summary>
        /// 销售单总数
        /// </summary>
        public float OrderCount { get; set; }

        /// <summary>
        /// 销售总额
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 退换货总数
        /// </summary>
        public float RMACount { get; set; }

        /// <summary>
        /// 退换货总额
        /// </summary>
        public decimal RMAAmount { get; set; }

        /// <summary>
        /// 经销售总额
        /// </summary>
        public decimal NetSalesAmount {
            get { return OrderAmount - RMAAmount; }
        }

        /// <summary>
        /// 待审核订单总数
        /// </summary>
        public int RequiredAuditOrderCount { get; set; }

        /// <summary>
        /// 待审核退换货单总数
        /// </summary>
        public int RequiredAuidtRMAOrderCount { get; set; }

        /// <summary>
        /// 待出库单总数
        /// </summary>
        public int WaitingForDeliveryOrderCount { get; set; }

        /// <summary>
        /// 缺货订单总数
        /// </summary>
        public int OutOfStockOrderCount { get; set; }

    }
}
