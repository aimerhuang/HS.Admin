using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 分销配送结算单子子实体
    /// </summary>
    /// <remarks>
    /// 2013-09-16 郑荣华 创建
    /// </remarks>
    public class PrtDsWhStockOutItem : WhStockOutItem
    {
        /// <summary>
        /// 商城订单号,分销
        /// </summary>
        public string MallOrderId { get; set; }

        /// <summary>
        /// 商品ERPCODE
        /// </summary>
        public string ErpCode { get; set; }

        /// <summary>
        /// 支付方式编号
        /// </summary>
        public int PayTypeSysNo { get; set; }

        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string PaymentName { get; set; }

        /// <summary>
        /// 支付方式类型
        /// </summary>
        public int PaymentType { get; set; }
    }
}
