using System;
using System.Collections.Generic;
using System.Linq;

namespace Hyt.Admin.Models
{
    #region 页面实体,对应一个共享页面
    /// <summary>
    /// 1.退货单维护列表
    /// </summary>
    /// <remarks>2013－07-11 黄志勇 创建</remarks>
    public class ReturnList
    {
        /// <summary>
        /// 退货单号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 销售单号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 出库单号
        /// </summary>
        public int OutStoreSysNo { get; set; }
        /// <summary>
        /// 支付金额(应退金额?)
        /// </summary>
        public decimal OrginAmount { get; set; }
        /// <summary>
        /// 退还金额
        /// </summary>
        public decimal RefundAmount { get; set; }
        /// <summary>
        /// 退货单状态
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 退货到(仓库|门店)
        /// </summary>
        public string WarehouseType { get; set; }
    }

    /// <summary>
    /// 2.门店退货小票
    /// </summary>
    /// <remarks>2013－07-11 黄志勇 创建</remarks>
    public class ReturnShopInvoice
    {
        /// <summary>
        /// 门店名称
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 退货单号
        /// </summary>
        public int RmaId { get; set; }
        /// <summary>
        /// 原订单号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string RecieveName { get; set; }
        /// <summary>
        /// 退款方式
        /// </summary>
        public int RefundType { get; set; }
        /// <summary>
        /// 退货时间
        /// </summary>
        public DateTime ReturnTime { get; set; }
        
        /// <summary>
        /// 退货明细
        /// </summary>
        public List<RcReturnItemEx> RcReturnItemExList { get; set; }

        /// <summary>
        /// 明细表退款金额合计
        /// </summary>
        public decimal RefundTotal
        {
            get
            {
                decimal amount = 0;
                if (RcReturnItemExList != null)
                {
                    amount = RcReturnItemExList.Sum(o => o.RefundAmount);
                }
                return amount;
            }
        }

    }

    /// <summary>
    /// 3.退货入库单
    /// </summary>
    /// <remarks>2013－07-11 黄志勇 创建</remarks>
    public class ReturnInStore
    {
        /// <summary>
        /// 入库单号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 退货单号
        /// </summary>
        public int RmaId { get; set; }
        /// <summary>
        /// 原订单号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string RecieveName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string MobilePhoneNumber { get; set; }
        /// <summary>
        /// 退款方式
        /// </summary>
        public int RefundType { get; set; }
        /// <summary>
        /// 入库仓库
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 退换货子表列表
        /// </summary>
        public List<RcReturnItemEx> RcReturnItemExList { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime InStoreDate { get; set; }
    }

    /// <summary>
    /// 4.新建退换货单
    /// </summary>
    /// <remarks>2013－07-11 黄志勇 创建</remarks>
    public class ReturnOrderList
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string RecieveName { get; set; }
        /// <summary>
        /// 会员姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 会员帐号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 配送方式
        /// </summary>
        public string DeliveryTypeName { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PaymentName { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public string CashPay { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public string PointPay { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string OrderSource { get; set; }
    }

    /// <summary>
    /// 5.显示订单实体
    /// </summary>
    /// <remarks>2013－07-11 黄志勇 创建</remarks>
    public class ReturnOrder
    {
        /// <summary>
        /// 会员姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 会员手机
        /// </summary>
        public string MobilePhoneNumber { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        public string OrderSource { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 总金额中现金支付部分
        /// </summary>
        public decimal CashPay { get; set; }

        /// <summary>
        /// 销售单总金额
        /// </summary>
        public decimal OrderAmount { get; set; }
        
        /// <summary>
        /// 发票类型
        /// </summary>
        public int InvoiceTypeSysNo { get; set; }

        /// <summary>
        /// 发票名称
        /// </summary>
        public string InvoiceType { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string InvoiceTitle { get; set; }

        /// <summary>
        /// 发票备注
        /// </summary>
        public string InvoiceRemarks { get; set; }

        /// <summary>
        /// 发票代码
        /// </summary>
        public string InvoiceCode { get; set; }

        /// <summary>
        /// 发票号码
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 会员编号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// RMA销售单号
        /// </summary>
        public int RmaOrderSysNo { get; set; }
    }
    #endregion

    #region 非页面实体，不对应共享页面
    /// <summary>
    /// 退换货出库单明细数据
    /// </summary>
    /// <remarks>2013－07-11 黄志勇 创建</remarks>
    public class ReturnEditOutStore
    {
        /// <summary>
        /// 出库单号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 出库单金额
        /// </summary>
        public decimal StockOutAmount { get; set; }
        /// <summary>
        /// 签收时间
        /// </summary>
        public DateTime SignTime { get; set; }
        /// <summary>
        /// 出库单状态
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// 出库仓库
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 出库仓库
        /// </summary>
        public string BackWarehouseName { get; set; }

        /// <summary>
        /// 已签收并超过15天
        /// </summary>
        public bool IsSignExpire { get; set; }

        /// <summary>
        /// 退换货单出库单明细扩展
        /// </summary>
        public List<ReturnWhStockOutItemEx> ReturnWhStockOutItemEx { get; set; }
    }

    /// <summary>
    /// 退换货单出库单明细扩展
    /// </summary>
    /// <remarks>2013－07-11 黄志勇 创建</remarks>
    public class ReturnWhStockOutItemEx
    {
        /// <summary>
        /// 退换货明细编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 出库单明细编号(余勇添加)
        /// </summary>
        public int StockOutItemSysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品原单价
        /// </summary>
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int ProductQuantity { get; set; }
        /// <summary>
        /// 实际销售金额
        /// </summary>
        public decimal RealSalesAmount { get; set; }
        /// <summary>
        /// 可退(换)数量
        /// </summary>
        public int ProductQuantityAble { get; set; }
        /// <summary>
        /// 实际退（换）数量（余勇添加）
        /// </summary>
        public int RmaQuantity { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundAmount { get; set; }
        /// <summary>
        /// 退(换)货原因
        /// </summary>
        public string RmaReason { get; set; }

        /// <summary>
        /// 订单明细编号
        /// </summary>
        public int OrderItemSysNo { get; set; }

        /// <summary>
        /// 商品退款价格类型:原价(10),自定义价格(20)
        /// </summary>
        public int ReturnPriceType { get; set; }

        /// <summary>
        /// 商品销售类型：普通(10),团购(20),秒杀(30),抢购(40),拍卖(50),组合(60),赠品(90)
        /// </summary>
        public int ProductSalesType { get; set; }
    }

    /// <summary>
    /// 退换货子表扩展
    /// </summary>
    /// <remarks>2013－07-11 黄志勇 创建</remarks>
    public class RcReturnItemEx
    {
        /// <summary>
        /// 商品号
        /// </summary>
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int RmaQuantity { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public string Weight { get; set; } 
        /// <summary>
        /// 单价
        /// </summary>
        public decimal OriginPrice { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal RefundAmount { get; set; }
    }
    #endregion
}