using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 退货计算对象
    /// </summary>
    /// <remarks>2013-09-16 吴文强 创建</remarks>
    public class CBRcReturnCalculate
    {
        /// <summary>
        /// 是否取回发票
        /// </summary>
        public int IsPickUpInvoice { get; set; }

        /// <summary>
        /// 应退金额(自动计算应退金额,已扣出订单折扣金额和优惠券金额)
        /// </summary>
        public decimal OrginAmount { get; set; }

        /// <summary>
        /// 优惠券金额(用于当前计算时展示使用，不做计算)
        /// </summary>
        public decimal CouponAmount { get; set; }

        /// <summary>
        /// 应退积分(自动计算应退积分)
        /// </summary>
        public int OrginPoint { get; set; }

        /// <summary>
        /// 应退惠源币(自动计算应退惠源币)
        /// </summary>
        public decimal OrginCoin { get; set; }

        /// <summary>
        /// 发票扣款金额(无发票应扣款金额,人工修改为准)
        /// </summary>
        public decimal DeductedInvoiceAmount { get; set; }

        /// <summary>
        /// 现金补偿金额(当积分不够扣除时用现金补偿)
        /// </summary>
        public decimal RedeemAmount { get; set; }

        /// <summary>
        /// 实退商品金额(不包含遗失发票扣款金额，人工修改应退金额)
        /// </summary>
        public decimal RefundProductAmount { get; set; }

        /// <summary>
        /// 实退积分
        /// </summary>
        public int RefundPoint { get; set; }

        /// <summary>
        /// 实退惠源币(与现金补偿互斥)
        /// </summary>
        public decimal RefundCoin { get; set; }

        /// <summary>
        /// 是否显示发票过期提示
        /// </summary>
        public bool ShowInvoiceExpire {get;set;}

        /// <summary>
        /// 实退总金额(实退商品金额-发票扣款金额-现金补偿金额)
        /// </summary>
        public decimal RefundTotalAmount { get; set; }

        /// <summary>
        /// 订单明细退货金额 余勇修改为泛型以便实现json序列化
        /// </summary>
        public List<KeyValuePair<int, decimal>> OrderItemAmount { get; set; }

        /// <summary>
        /// 出库单明细退货金额 余勇修改为泛型以便实现json序列化
        /// </summary>
        public List<KeyValuePair<int, decimal>> StockOutItemAmount { get; set; }

    }

    public class StockOutItemRmaQuantity
    {
        public int StockOutItemSysNo { get; set; }
        public int RmaQuantity { get; set; }
    }
}
