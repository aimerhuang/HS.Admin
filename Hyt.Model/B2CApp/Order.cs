using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Model.B2CApp
{
    /// <summary>
    /// 订单明细
    /// </summary>
    /// <remarks>2013--8-6 杨浩 添加</remarks>
    public class OrderItem
    {
        /// <summary>
        /// 订购商品数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 商品原价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 真实价格
        /// </summary>
        public decimal LevelPrice { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// 商品规格
        /// </summary>
        public string Specification { get; set; }
    }

    /// <summary>
    /// 订单
    /// </summary>
    /// <remarks>2013-8-6 杨浩 添加</remarks>
    public class Order
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string OnlineStatus { get; set; }
        /// <summary>
        /// 订单商品明细
        /// </summary>
        public IList<OrderItem> Products { get; set; }
        /// <summary>
        /// 销售单总金额
        /// </summary>
        public decimal OrderAmount { get; set; }
        /// <summary>
        /// 明细数量
        /// </summary>
        public decimal OrderItemCount { get; set; }
        /// <summary>
        /// 可操作项
        /// </summary>
        public string OperateStatus { get; set; }
    }

    /// <summary>
    /// 订单详细
    /// </summary>
    /// <remarks>2013-8-6 杨浩 添加</remarks>
    public class OrderDetail
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 订单显示状态
        /// </summary>
        public string OnlineStatus { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public int PayStatus { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PaymentName { get; set; }

        /// <summary>
        /// 商品销售价合计
        /// </summary>
        public decimal ProductAmount { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal FreightAmount { get; set; }

        /// <summary>
        /// 销售单总金额
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 总优惠金额
        /// </summary>
        public decimal TotalDiscountAmount { get; set; }

        /// <summary>
        /// 配送前是否联系
        /// </summary>
        public int ContactBeforeDelivery { get; set; }

        /// <summary>
        /// 配送时间段
        /// </summary>
        public string DeliveryTime { get; set; }

        /// <summary>
        /// 配送方式名
        /// </summary>
        public string DeliveryTypeName { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public SoAddress ReceiveAddress { get; set; }

        /// <summary>
        /// 可操作项 enum
        /// </summary>
        //public string OperateStatus { get; set; }

        /// <summary>
        /// 订单商品明细
        /// </summary>
        public IList<OrderItem> Products { get; set; }
    }

    /// <summary>
    /// 收货地址
    /// </summary>
    /// <remarks>2013-8-6 杨浩 添加</remarks>
    public class SoAddress
    {
        /// <summary>
        /// 地区名
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 街道地址
        /// </summary>
        public string StreetAddress { get; set; }
    }

    /// <summary>
    /// 创建订单成功后 返回的提示
    /// </summary>
    /// <remarks>2013--8-6 杨浩 添加</remarks>
    public class OrderResult
    {
        /// <summary>
        /// 结算金额(=商品销售合计金额+运费金额-商品优惠金额-运费优惠金额-订单优惠金额)
        /// SettlementAmount = ProductAmount+FreightAmount-ProductDiscountAmount-FreightDiscountAmount-SettlementDiscountAmount-CouponAmount
        /// </summary>
        public decimal SettlementAmount { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderSysNo { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public PaymentType PaymentType { get; set; }
    }

    #region create

    /// <summary>
    /// 新建订单模型
    /// </summary>
    /// <remarks>2013-8-6 杨浩 添加</remarks>
    public class CreateOrder
    {
        /// <summary>
        /// 收货地址编号
        /// </summary>
        public int ReceiveAddressSysNo { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 默认仓库编号
        /// </summary>
        public int DefaultWarehouseSysNo { get; set; }

        /// <summary>
        /// 配送方式编号
        /// </summary>
        public int DeliveryTypeSysNo { get; set; }

        /// <summary>
        /// 支付方式编号
        /// </summary>
        public int PayTypeSysNo { get; set; }

        /// <summary>
        /// 配送时间段
        /// </summary>
        public string DeliveryTime { get; set; }

        /// <summary>
        /// 配送前是否联系
        /// </summary>
        public bool ContactBeforeDelivery { get; set; }

        /// <summary>
        /// 优惠劵码
        /// </summary>
        public string CouponCode { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public int? OrderSource { get; set; }

    }

    #endregion

    /// <summary>
    /// 订单日志查询
    /// </summary>
    /// <remarks>2013-8-6 杨浩 添加</remarks>
    public class TransactionLog
    {
        /// <summary>
        /// 配送方式Name
        /// </summary>
        public string DeliveryTypeName { get; set; }

        /// <summary>
        /// 支付方式Name
        /// </summary>
        public string PayTypeName { get; set; }

        /// <summary>
        /// 配送单单号
        /// </summary>
        public string DeliverySysNo { get; set; }

        /// <summary>
        /// 订单日志
        /// </summary>
        public IList<SoTransactionLog> Transactions { get; set; }
    }
}
