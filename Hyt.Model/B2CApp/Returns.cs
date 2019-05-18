using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.B2CApp
{
    #region 退换货模型

    /// <summary>
    /// 退换货详细
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ReturnDetail
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 出库单编号
        /// </summary>
        public int StockOutSysNo { get; set; }
        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string OnlineStatus { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal OrderAmount { get; set; }
        /// <summary>
        /// 可退换货商品列表
        /// </summary>
        public IList<ProductReturn> ProductReturns { get; set; }
        /// <summary>
        /// 是否是在线支付
        /// </summary>
        public bool IsOnlinePay { get; set; }
    }

    /// <summary>
    /// 可退换货商品模型
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ProductReturn
    {
        /// <summary>
        /// 出库单明细编号
        /// </summary>
        public int StockOutItemSysNo { get; set; }

        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 实际价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 用户最大可退换数量
        /// </summary>
        public int MaxReturnQuantity { get; set; }

        /// <summary>
        /// 用户退换货数量
        /// </summary>
        public int RmaQuantity { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }
    }

    /// <summary>
    /// 退换货明细
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ProductReturnSysNo
    {
        /// <summary>
        /// 出库单明细编号
        /// </summary>
        public int StockOutItemSysNo { get; set; }

        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 用户退换货数量
        /// </summary>
        public int RmaQuantity { get; set; }
    }

    /// <summary>
    /// 新增换货模型
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ExchangeOrders
    {
        /// <summary>
        /// 客户编号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 退换货明细
        /// </summary>
        public IList<ProductReturnSysNo> ProductReturns { get; set; }

        /// <summary>
        /// 退换货原因
        /// </summary>
        public string RmaReason { get; set; }

        /// <summary>
        /// 取件方式
        /// </summary>
        public int PickupTypeSysNo { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public int WarehouseSysNo { get; set; }

        /// <summary>
        /// 取件地址编号
        /// </summary>
        public int PickUpAddressSysNo { get; set; }

        /// <summary>
        /// 收货地址编号(-10：与原收货地址相同)
        /// </summary>
        public int ReceiveAddressSysNo { get; set; }
    }

    /// <summary>
    /// 退货单模型
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class RejectedOrders : ExchangeOrders
    {
        /// <summary>
        /// 是否取回发票
        /// </summary>
        public int IsPickUpInvoice { get; set; }

        /// <summary>
        /// 是否在线支付
        /// </summary>
        public bool IsOnlinePay { get; set; }
        
        /// <summary>
        /// 收款人开户行
        /// </summary>
        public string RefundBank { get; set; }

        /// <summary>
        /// 收款人开户姓名
        /// </summary>
        public string RefundAccountName { get; set; }

        /// <summary>
        /// 收款人银行账号
        /// </summary>
        public string RefundAccount { get; set; }
    }

    /// <summary>
    /// 取件方式
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class RePickupType
    {
        /// <summary>
        /// 取件方式编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 取件方式名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
    }

    /// <summary>
    /// 退换货历史
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ReturnHistory
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// RMA类型
        /// </summary>
        public int RmaType { get; set; }

        /// <summary>
        /// 实退总金额
        /// </summary>
        public string RefundTotalAmount { get; set; }

        /// <summary>
        /// 实退积分
        /// </summary>
        public string RefundPoint { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 状态文本
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// 待审核(10),待入库(20),待退款(30),已完成(50),作废(-10)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 商品明细
        /// </summary>
        public IList<ReturnHistoryItem> Items { get; set; }
    }

    /// <summary>
    /// 退换货历史明细
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ReturnHistoryItem
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }
        /// <summary>
        /// 退换货数量
        /// </summary>
        public int RmaQuantity { get; set; }
    }

    /// <summary>
    /// 退换货查询
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ReturnHistorySub : ReturnHistory
    {
        /// <summary>
        /// 日志记录
        /// </summary>
        public IList<RcReturnLog> Logs { get; set; }
    }

    #endregion
}
