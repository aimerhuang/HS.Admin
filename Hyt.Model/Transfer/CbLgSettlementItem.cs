using System;
using System.Collections.Generic;

namespace Hyt.Model.Transfer
{

    /// <summary>
    /// 结算单明细
    /// </summary>
    /// <remarks>2013-06-28 黄伟 创建</remarks>
    public class CBLgSettlementItem : LgSettlementItem
    {
 
        /// <summary>
        /// 订单系统编号
        /// </summary>
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 配送员系统编号
        /// </summary>
        public int DeliveryUserSysNo { get; set; }

        /// <summary>
        /// 配送单明细状态
        /// </summary>
        public int SignStatus { get; set; }

        /// <summary>
        /// 结算支付明细
        /// </summary>
        public IList<PayItem> PayItemList { get; set; }

 
        /// <summary>
        ///  商品号,签收数量
        /// </summary>
        /// <value>
        /// 商品号,签收数量
        /// </value>
        public Dictionary<int, int> SignNumber { get; set; }

        /// <summary>
        /// 需要退货的订单信息-重新计算价格
        /// </summary>
        public CBRMAOrderInfo RMAOrderInfo { get; set; }

        /// <summary>
        /// 浅CLONE
        /// </summary>
        /// <returns></returns>
        public LgSettlementItem Clone() 
        {
            return this.MemberwiseClone() as LgSettlementItem;
        }
    }

    /// <summary>
    /// 结算支付明细
    /// </summary>
    /// <remarks>
    /// 2013-07-11 何方 创建
    /// </remarks>
    public class PayItem
    {
        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 支付单号
        /// </summary>
        public string PayNo { get; set; }

        /// <summary>
        /// 刷卡卡号
        /// </summary>
        /// <value>1234****6789</value>
        public string PosCardNo { get; set; }

        /// <summary>
        /// 收款科目
        /// </summary>
        public string EasCode { get; set; }
    }
}

	