using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.UpGrade
{
    /// <summary>
    /// 第三方订单实体
    /// </summary>
    /// <remarks>2013-8-28 杨浩 创建</remarks>
    public class UpGradeOrder
    {
        /// <summary>
        /// 第三方订单交易信息
        /// </summary>
        public MallOrderPaymentInfo MallOrderPayment { get; set; }

        /// <summary>
        /// 商城订单相关信息
        /// </summary>
        public HytOrderDealerInfo HytOrderDealer { get; set; }

        /// <summary>
        /// 第三方买家订单信息
        /// </summary>
        public MallOrderBuyerInfo MallOrderBuyer { get; set; }

        /// <summary>
        /// 订单收货信息
        /// </summary>
        public MallOrderReceiveInfo MallOrderReceive { get; set; }

        /// <summary>
        /// 订单明细列表
        /// </summary>
        public List<UpGradeOrderItem> UpGradeOrderItems { get; set; }

    }

    /// <summary>
    /// 第三方订单交易信息
    /// </summary>
    public class MallOrderPaymentInfo
    {
        /// <summary>
        /// 第三方订单支付时间
        /// </summary>
        public DateTime PayTime { get; set; }

        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string AlipayNo { get; set; }

        /// <summary>
        /// 第三方订单支付金额
        /// </summary>
        public decimal Payment { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal PostFee { get; set; }

        /// <summary>
        /// 升舱服务费
        /// </summary>
        public decimal ServiceFee { get; set; }

        /// <summary>
        /// 优惠总金额
        /// </summary>
        public decimal DiscountFee { get; set; }

        /// <summary>
        /// 总税费金额
        /// </summary>
        public decimal TotalTaxAmount { get; set; }
    }

    /// <summary>
    /// 商城订单相关信息
    /// </summary>
    public class HytOrderDealerInfo
    {
        /// <summary>
        /// 分销商编号
        /// </summary>
        public int DealerSysNo { get; set; }

        /// <summary>
        /// 分销商商城系统编号
        /// </summary>
        public int DealerMallSysNo { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 商城升舱订单信息系统编号
        /// </summary>
        public int DsOrderSysNo { get; set; }

        /// <summary>
        /// 商城支付状态 (10 未支付 20 已支付)
        /// </summary>
        public int HytPayStatus { get; set; }

        /// <summary>
        /// 商城支付方式
        /// </summary>
        public int HytPayType { get; set; }

        /// <summary>
        /// 商城支付时间
        /// </summary>
        public DateTime HytPayTime { get; set; }

        /// <summary>
        /// 商城订单支付金额
        /// </summary>
        public decimal HytPayment { get; set; }

        /// <summary>
        /// 商城订单号
        /// </summary>
        public string HytOrderId { get; set; }
        /// <summary>
        /// 商城订单事物编号
        /// </summary>
        public string OrderTransactionSysNo { get; set; }


        /// <summary>
        /// 商城配送状态
        /// </summary>
        public string DeliveryStatus { get; set; }

        /// <summary>
        /// 商城订单时间，即升舱时间
        /// </summary>
        public DateTime HytOrderTime { get; set; }

        /// <summary>
        /// 商城订单发货时间
        /// </summary>
        public DateTime LogisticsTime { get; set; }

        /// <summary>
        /// 商城备注
        /// </summary>
        public string DealerMessage { get; set; }

        /// <summary>
        /// 是否使用预存款
        /// </summary>
        public int IsPreDeposit { get; set; }
        /// <summary>
        ///     是否自营
        /// </summary>
        public int IsSelfSupport { get; set; }
    }

    /// <summary>
    /// 第三方订单信息
    /// </summary>
    public class MallOrderBuyerInfo
    {
        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string MallOrderId { get; set; }

        /// <summary>
        /// 第三方SN订单号
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 第三方分销采购单交易号，采购单发货时用
        /// </summary>
        public string MallPurchaseId { get; set; }

        /// <summary>
        /// 第三方买家昵称
        /// </summary>
        public string BuyerNick { get; set; }

        /// <summary>
        /// 买家留言
        /// </summary>
        public string BuyerMessage { get; set; }

        /// <summary>
        /// 第三方卖家备注
        /// </summary>
        public string SellerMessage { get; set; }


        public string SellerFlag { get; set; }



    }

    /// <summary>
    /// 订单收货信息
    /// </summary>
    public class MallOrderReceiveInfo
    {
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string ReceiveContact { get; set; }

        /// <summary>
        /// 收货手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 收货座机
        /// </summary>
        public string TelPhone { get; set; }

        /// <summary>
        /// 收货邮编
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// 收货商城省市区编号
        /// </summary>
        public int AreaSysNo { get; set; }

        /// <summary>
        /// 收货省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 收货市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 收货区
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceiveAddress { get; set; }
    }

    /// <summary>
    /// 商城订单日志
    /// </summary>
    /// <remarks>2013-09-06 朱家宏 创建</remarks>
    public class HytOrderLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public string TransactionSysNo { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogContent { get; set; }
        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateDate { get; set; }
    }
}
