using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using System.ComponentModel;

namespace Hyt.Model.ExpressList
{
    /// <summary>
    /// 快递单表 
    /// </summary>
    /// <remarks> 2017-11-24 廖移凤 创建</remarks>
    public class ExpressLists
    {
        /// <summary>
        ///快递单表 编号
        /// </summary>
        [Description("快递单表编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 出库单编号
        /// </summary>
        [Description("出库单编号")]
        public int WhStockInId { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [Description("订单编号")]
        public string OrderSysNo { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        [Description("快递单号")]
        public string ExpressListNo { get; set; }

    }
}
