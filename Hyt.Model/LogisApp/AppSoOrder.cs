using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// 用于App接口的订单对象
    /// </summary>
    /// <remarks>2013-12-01 沈强 创建</remarks>
    public class AppSoOrder
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 订单收货地址系统编号
        /// </summary>
        public int ReceiveAddressSysNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public string TransactionSysNo { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 默认仓库编号
        /// </summary>
        public int DefaultWarehouseSysNo { get; set; }
        /// <summary>
        /// 配送配送方式编号
        /// </summary>
        public int DeliveryTypeSysNo { get; set; }
        /// <summary>
        /// 配送配送方式名称
        /// </summary>
        public string DeliveryTypeName { get; set; }
        /// <summary>
        /// 支付方式编号
        /// </summary>
        public int PayTypeSysNo { get; set; }
        /// <summary>
        /// 商品销售价合计(明细销售单价*数量,之和.优惠前金额)
        /// </summary>
        public decimal ProductAmount { get; set; }
        /// <summary>
        /// 商品折扣金额(明细折扣金额之和)
        /// </summary>
        public decimal ProductDiscountAmount { get; set; }
        /// <summary>
        /// 商品调价金额合计(明细调价金额之和)
        /// </summary>
        public decimal ProductChangeAmount { get; set; }
        /// <summary>
        /// 运费折扣金额
        /// </summary>
        public decimal FreightDiscountAmount { get; set; }
        /// <summary>
        /// 运费调价金额(正负值)
        /// </summary>
        public decimal FreightChangeAmount { get; set; }
        /// <summary>
        /// 已使用促销系统编号(多个促销分号分隔)
        /// </summary>
        public string UsedPromotions { get; set; }
        /// <summary>
        /// 销售单总金额(商品销售价合计+运费-商品折扣金额-运费
        /// </summary>
        public decimal OrderAmount { get; set; }
        /// <summary>
        /// 运费金额(折扣前金额)
        /// </summary>
        public decimal FreightAmount { get; set; }
        /// <summary>
        /// 订单折扣金额(订单折扣金额，不包含商品折扣金额)
        /// </summary>
        public decimal OrderDiscountAmount { get; set; }
        /// <summary>
        /// 优惠券金额(多张优惠券合计)
        /// </summary>
        public decimal CouponAmount { get; set; }
        /// <summary>
        /// 总金额中现金支付部分
        /// </summary>
        public decimal CashPay { get; set; }
        /// <summary>
        /// 总金额中惠源币支付部分
        /// </summary>
        public decimal CoinPay { get; set; }
        /// <summary>
        /// 发票系统编号
        /// </summary>
        public int InvoiceSysNo { get; set; }
        /// <summary>
        /// 下单来源：PC网站（10）、信营全球购B2B2C3G网站（15）
        /// </summary>
        public int OrderSource { get; set; }
        /// <summary>
        /// 下单来源编号
        /// </summary>
        public int OrderSourceSysNo { get; set; }
        /// <summary>
        /// 销售方式：普通订单（10）、调价订单（40）、经销订单
        /// </summary>
        public int SalesType { get; set; }
        /// <summary>
        /// 销售方式编号
        /// </summary>
        public int SalesSysNo { get; set; }
        /// <summary>
        /// 此销售单对用户是否隐藏：是（1）、否（0）
        /// </summary>
        public int IsHiddenToCustomer { get; set; }
        /// <summary>
        /// 支付状态：未支付（10）、已支付（20）、支付异常（30
        /// </summary>
        public int PayStatus { get; set; }
        /// <summary>
        /// 客户留言
        /// </summary>
        public string CustomerMessage { get; set; }
        /// <summary>
        /// 对内备注
        /// </summary>
        public string InternalRemarks { get; set; }
        /// <summary>
        /// 配送备注
        /// </summary>
        public string DeliveryRemarks { get; set; }
        /// <summary>
        /// 配送时间段
        /// </summary>
        public string DeliveryTime { get; set; }
        /// <summary>
        /// 配送前是否联系：是（1）、否（0）
        /// </summary>
        public int ContactBeforeDelivery { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 下单人编号（系统用户）
        /// </summary>
        public int OrderCreatorSysNo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 销售单审核人编号
        /// </summary>
        public int AuditorSysNo { get; set; }
        /// <summary>
        /// 销售单审核时间
        /// </summary>
        public DateTime AuditorDate { get; set; }
        /// <summary>
        /// 销售单作废人类型:前台用户(10),后台用户(20)
        /// </summary>
        public int CancelUserType { get; set; }
        /// <summary>
        /// 销售单作废人编号
        /// </summary>
        public int CancelUserSysNo { get; set; }
        /// <summary>
        /// 销售单作废时间
        /// </summary>
        public DateTime CancelDate { get; set; }
        /// <summary>
        /// 前台显示状态
        /// </summary>
        public string OnlineStatus { get; set; }
        /// <summary>
        /// 状态：待审核（10）、待支付（20）、待创建出库单（30
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateDate { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Stamp { get; set; }

        /// <summary>
        /// 订单发票信息
        /// </summary>
        /// <remarks>2013-06-21 朱成果 创建</remarks>
        public AppFnInvoice OrderInvoice { get; set; }

        /// <summary>
        /// 关联收货地址
        /// </summary>
        public AppSoReceiveAddress ReceiveAddress { get; set; }

        /// <summary>
        /// 订购商品列表
        /// </summary>
        /// <remarks>2013-06-19 朱成果 创建</remarks>
        public IList<Hyt.Model.LogisApp.AppSoOrderItem> OrderItemList { get; set; }

        /// <summary>
        /// 凭证号
        /// </summary>
        public string VoucherNo { get; set; }
        /// <summary>
        /// 交易(银行卡,信用卡)卡号
        /// </summary>
        public string CreditCardNumber { get; set; }

        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string PaymentTypeName { get; set; }

        /// <summary>
        /// 支付方式类型
        /// </summary>
        public int PaymentType { get; set; }

        /// <summary>
        /// 支付方式是否需要卡号：是(1),否(0)
        /// </summary>
        public int RequiredCardNumber { get; set; }

        /// <summary>
        /// 优惠卷集合
        /// </summary>
        public List<AppSpCoupon> AppSpCoupons { get; set; }
    }

    /// <summary>
    /// 用于App接口的订单明细对象
    /// </summary>
    /// <remarks>2013-12-01 沈强 创建</remarks>
    public class AppSoOrderItem
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 销售单系统编号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 事物编号(订单)
        /// </summary>
        public string TransactionSysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 订购数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 原单价：商品会员等级价格
        /// </summary>
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 销售单价(不包含优惠金额)
        /// </summary>
        public decimal SalesUnitPrice { get; set; }
        /// <summary>
        /// 销售金额(销售单价*数量)
        /// </summary>
        public decimal SalesAmount { get; set; }
        /// <summary>
        /// 折扣金额(商品分摊折扣金额)
        /// </summary>
        public decimal DiscountAmount { get; set; }
        /// <summary>
        /// 调价金额(正负调价金额)
        /// </summary>
        public decimal ChangeAmount { get; set; }
        /// <summary>
        /// 实际出库数量(创建出库单数量)
        /// </summary>
        public int RealStockOutQuantity { get; set; }
        /// <summary>
        /// 商品销售类型：普通(10),团购(20),秒杀(30),抢购(40),
        /// </summary>
        public int ProductSalesType { get; set; }
        /// <summary>
        /// 商品销售类型编号
        /// </summary>
        public int ProductSalesTypeSysNo { get; set; }
        /// <summary>
        /// 组代码(组合,团购时使用)
        /// </summary>
        public string GroupCode { get; set; }
        /// <summary>
        /// 组名称(组合,团购时使用)
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 已使用促销系统编号(多个促销分号分隔)
        /// </summary>
        public string UsedPromotions { get; set; }
        /// <summary>
        /// 商品缩略图
        /// </summary>
        public string ProductImage { get; set; }
    }
}
