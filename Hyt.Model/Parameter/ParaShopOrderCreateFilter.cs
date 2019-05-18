using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    #region 门店下单支付提货
    /// <summary>
    /// 门店订单创建参数字段
    /// </summary>
    /// <remarks>2013-06-28 余勇 创建</remarks>
    public class ParaShopOrderCreateFilter
    {
        /// <summary>
        /// 门店SysNo
        /// </summary>
        public int shopSysNo { get; set; }
        /// <summary>
        /// 会员SysNo.
        /// </summary>
        public int customerSysNo { get; set; }
        /// <summary>
        /// 收货人姓名..
        /// </summary>
        public string receiveName { get; set; }
        /// <summary>
        /// 收货人手机号码.
        /// </summary>
        public string receiveMobilePhoneNumber { get; set; }
        /// <summary>
        /// 备注..
        /// </summary>
        public string internalRemarks { get; set; }
        /// <summary>
        /// 订购商品清单
        /// </summary>
        public IList<Model.SoOrderItem> orderItem { get; set; }
        /// <summary>
        /// 发票类型
        /// </summary>
        public int invoiceType { get; set; }
        /// <summary>
        /// 发票代码
        /// </summary>
        public string invoiceCode { get; set; }
        /// <summary>
        /// 发票号码
        /// </summary>
        public string invoiceNo { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string invoiceTitle { get; set; }
        /// <summary>
        /// 支付积分
        /// </summary>
        public string pointPay { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayMoney { get; set; }
        /// <summary>
        /// 支付惠源币
        /// </summary>
        public int CoinPay { get; set; }
        /// <summary>
        /// 刷卡流水号(收款单明细-交易凭证号)
        /// </summary>
        /// <remarks>2013-10-14 朱家宏 添加</remarks>
        public string VoucherNo { get; set; }

        /// <summary>
        /// EAS收款科目编码
        /// </summary>
        /// <remarks>2013-11-15 朱成果 添加</remarks>
        public string EasReceiptCode { get; set; }
        /// <summary>
        /// 优惠券编码
        /// </summary>
        public string CouponCode { get; set; }
        /// <summary>
        /// 会员卡余额
        /// </summary>
        public decimal Balance { get; set; }
    }
    #endregion

    #region 门店下单延迟处理
    /// <summary>
    /// 门店下单延迟处理参数字段
    /// </summary>
    /// <remarks>2013-06-28 余勇 创建</remarks>
    public class ParaShopOrderDelayFilter : ParaShopOrderCreateFilter
    {
        /// <summary>
        /// 延迟原因
        /// </summary>
        public string delayReason { get; set; }
        /// <summary>
        /// 延迟提货日期
        /// </summary>
        public DateTime pickUpDate{ get; set; }
    }
    #endregion

    #region 门店下单提交快递送达
    /// <summary>
    /// 门店下单提交快递送达参数字段
    /// </summary>
    /// <remarks>2013-06-28 余勇 创建</remarks>
    public class ParaShopOrderShipFilter : ParaShopOrderCreateFilter
    {
        /// <summary>
        /// 地区编号
        /// </summary>
        public int areaSysNo { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string streetAddress { get; set; }
        /// <summary>
        /// 转快递原因
        /// </summary>
        public string shipReson { get; set; }
        /// <summary>
        /// 会员留言
        /// </summary>
        public string customerMessage { get; set; }

        /// <summary>
        /// 会员接收地址编号
        /// </summary>
        public int ReceiveAddressSysNo { get; set; }
    }
    #endregion
}
