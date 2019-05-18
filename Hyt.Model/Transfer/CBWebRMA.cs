using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{

    /// <summary>
    /// 前台退换货传输对象
    /// </summary>
    /// <remarks>
    /// 该对象只用于前台网站使用
    /// 
    /// 2013-08-27 邵斌 创建
    /// </remarks>
    [Serializable]
    public class CBWebRMA
    {
        /// <summary>
        /// 客户系统编号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 退换货系统编号
        /// </summary>
        public int ReturnSysNo { get; set; }

        /// <summary>
        /// 订单系统编号
        /// </summary>
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public SoReceiveAddress PickUpAddress { get; set; }

        /// <summary>
        /// 取件方式
        /// </summary>
        public int PickUpType { get; set; }

        /// <summary>
        /// 百城当日取件等级（4:普通取件，5:加急取件，6:定时取件）
        /// </summary>
        public int PickUpLevel { get; set; }

        /// <summary>
        /// 定时取件时间(条件：PickUpTy = 百城当日取件，PickUpLevel = 定时取件)
        /// </summary>
        public DateTime PickUpTime { get; set; }

        /// <summary>
        /// 可用商品列表
        /// </summary>
        public IList<CBWebRMAItem> Items { get; set; }

        /// <summary>
        /// 发票系统编号 
        /// </summary>
        public int InvoiceSysNo { get; set; }

        /// <summary>
        /// 是否有发票
        /// </summary>
        public bool HasInvoice { get; set; }

        /// <summary>
        /// 发票状态
        /// </summary>
        public Hyt.Model.WorkflowStatus.FinanceStatus.发票状态 InvoiceStatus { get; set; }

        /// <summary>
        /// 退换货原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public SoReceiveAddress ReceiveAddress { get; set; }

        /// <summary>
        /// 退换方式
        /// </summary>
        public int ReturnType { get; set; }

        /// <summary>
        /// 配送方式（仅换货是有用）
        /// </summary>
        public int ShipType { get; set; }

        /// <summary>
        /// 发票取回类型
        /// </summary>
        public int PickUpInvoiceType { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        public BankInfo Bank { get; set; }

        /// <summary>
        /// 是否是原路返还费用
        /// </summary>
        public bool IsBacktracked { get; set; }

        /// <summary>
        /// 退换货申请时间
        /// </summary>
        public DateTime ApplyDateTime { get; set; }

        /// <summary>
        /// 退换货搜索关键字
        /// </summary>
        public string SearchKeyWords { get; set; }

        /// <summary>
        /// 退换货状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 退换货仓库系统编号
        /// </summary>
        public int WarehouseSysNo { get; set; }
    }

    /// <summary>
    /// 前台退换货传输对象 商品明细
    /// </summary>
    /// <remarks>
    /// 该对象只用于前台网站使用
    /// 
    /// 2013-08-27 邵斌 创建
    /// </remarks>
    [Serializable]
    public class CBWebRMAItem
    {
        /// <summary>
        /// 退换货系统编号
        /// </summary>
        public int ReturnSysNo { get; set; }

        /// <summary>
        /// 出库单明细系统编号
        /// </summary>
        public int StockOutItemSysNo { get; set; }

        /// <summary>
        /// 销售单明细系统编号
        /// </summary>
        public int OrderItemSysNo { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 包装清单
        /// </summary>
        public string PackageDesc { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string Image { get; set; }

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
        /// 商品是否可提交退（换）货申请
        /// </summary>
        public bool EnableRMA { get; set; }

        /// <summary>
        /// 价格优惠
        /// </summary>
        public decimal Preferential { get; set; }
    }

    /// <summary>
    /// 银行信息
    /// </summary>
    /// <remarks>2013-08-30 邵斌 创建</remarks>
    [Serializable]
    public class BankInfo
    {
        /// <summary>
        /// 开户行
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Number { get; set; }
    }
}
