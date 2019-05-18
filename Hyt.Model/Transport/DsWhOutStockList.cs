using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    public class CBDsWhOutStockList : DsWhOutStockList
    {
        public string CustomsNum { get; set; }
        public string ReceiptPageName { get; set; }
        /// <summary>
        /// 发货人
        /// </summary>
        public string SendUser { get; set; }
        /// <summary>
        /// 发货地址
        /// </summary>
        public string SendAddress { get; set; }
        /// <summary>
        /// 发货人号码
        /// </summary>
        public string SendTelephone { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string Receipter { get; set; }
        /// <summary>
        /// 收货人身份证
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 收货人联系电话
        /// </summary>
        public string ReceiptTele { get; set; }
        /// <summary>
        /// 收货人所在城市
        /// </summary>
        public string ReceiptCity { get; set; }
        /// <summary>
        /// 收货人编码
        /// </summary>
        public string ReceiptMall { get; set; }
        /// <summary>
        /// 收货人地址
        /// </summary>
        public string ReceiptAddress { get; set; }
        public decimal TotalValue { get; set; }
        /// <summary>
        /// 保价金额
        /// </summary>
        public decimal Valuation { get; set; }
        /// <summary>
        /// 包裹重量
        /// </summary>
        public decimal WeightValue { get; set; }
        /// <summary>
        /// 币别
        /// </summary>
        public string Currency { get; set; }
        public string Dis { get; set; } 
    }
    /// <summary>
    /// 出库单操作明细
    /// </summary>
    /// <remarks>
    /// 2016-5-16 杨云奕 添加
    /// </remarks>
    public class DsWhOutStockList
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 出库单编号
        /// </summary>
        public int PSysNo { get; set; }
        /// <summary>
        /// 货物包裹编号
        /// </summary>
        public string GMSysNo { get; set; }
        /// <summary>
        /// 货物包裹快递单号
        /// </summary>
        public string GMCourierNumber { get; set; }
        /// <summary>
        /// 件内名称
        /// </summary>
        public string GMReceiptPageName { get; set; }
        /// <summary>
        /// 件内总重量
        /// </summary>
        public decimal GMWeightValue { get; set; }
        /// <summary>
        /// 计算重量
        /// </summary>
        public decimal CalculateWeight { get; set; }
        /// <summary>
        /// 计算单价
        /// </summary>
        public decimal CalculatePrice { get; set; }
        /// <summary>
        /// 打折
        /// </summary>
        public decimal CalculateDiscount { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal CalculateTotalValue { get; set; }
    }
}
