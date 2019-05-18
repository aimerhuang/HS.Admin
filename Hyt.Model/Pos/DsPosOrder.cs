using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    public class DBDsPosOrderItemData : DsPosOrder
    {
        
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProName { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        public string ProBarCode { get; set; }
        /// <summary>
        /// 商品原价
        /// </summary>
        public decimal ProPrice { get; set; }
        /// <summary>
        /// 商品销售数量
        /// </summary>
        public int ProNum { get; set; }
        /// <summary>
        /// 商品销售折扣
        /// </summary>
        public decimal ProDiscount { get; set; }
        /// <summary>
        /// 商品折扣金额
        /// </summary>
        public decimal ProDisPrice { get; set; }
        /// <summary>
        /// 总销售金额
        /// </summary>
        public decimal ProTotalValue { get; set; }
    }
    /// <summary>
    /// Pos订单对象
    /// </summary>
    public class DBDsPosOrder : DsPosOrder
    {
        public decimal CouponAmount { get; set; }
        public string StoreName { get; set; }
        public string PosName { get; set; }
        public string DealerName { get; set; }
        public List<DsPosOrderItem> items = new List<DsPosOrderItem>();

        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }
        public decimal OrginPrice { get; set; }
        public decimal DisPrice { get; set; }
        public decimal Price { get; set; }
        public string SellTime { get; set; }
        public string SaleType { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string returnSaleNumber { get; set; }
        public string returnSaleTotal { get; set; }
        public string returnQuantity { get; set; }
        public string returnPrice { get; set; }
    }

    public class WareDsPosOrder : DsPosOrder
    {
        public int WarehouseSysNo { get; set; }
    }
    /// <summary>
    /// Pos机销售单
    /// </summary>
    public class DsPosOrder
    {
        public int SysNo { get; set; }
        /// <summary>
        /// 经销商编码
        /// </summary>
        public int DsSysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int DsPosSysNo { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 营业员
        /// </summary>
        public string Clerker { get; set; }
        /// <summary>
        /// 销售时间
        /// </summary>
        public string SaleTime { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalNum { get; set; }
        /// <summary>
        /// 总销售金额
        /// </summary>
        public decimal TotalSellValue { get; set; }
        /// <summary>
        /// 总原价金额
        /// </summary>
        public decimal TotalOrigValue { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal TotalDisValue { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal TotalPayValue { get; set; }
        /// <summary>
        /// 找零
        /// </summary>
        public decimal TotalGetValue { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// 会员卡名称
        /// </summary>
        public string CardName { get; set; }
        /// <summary>
        /// 会员卡订单折扣
        /// </summary>
        public decimal HavePrivilege { get; set; }
        /// <summary>
        /// 订单返积分
        /// </summary>
        public decimal OrderPoint { get; set; }
        /// <summary>
        /// 抵消金额的积分
        /// </summary>
        public decimal UsePoint { get; set; }
        /// <summary>
        /// 抵消金额的积分转金额
        /// </summary>
        public decimal PointMoney { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// 订单单状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 付款码数据
        /// </summary>
        public string PayAuthCode { get; set; }
        /// <summary>
        /// 优惠卷编码
        /// </summary>
        public int CouponSysNo
        {
            get;
            set;
        }
        /// <summary>
        /// 优惠卷描述
        /// </summary>
        public string CouponSysDis { get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime PayTime { get; set; }
        public string ResturnMsg { set; get; }
    }
}
