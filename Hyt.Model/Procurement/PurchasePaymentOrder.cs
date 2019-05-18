using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    public class CBPurchasePaymentOrder : FnPurchasePaymentOrder
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string BankPaymentInfo { get; set; }
        public List<CBFnPurchasePaymentOrderItem> PurchaseOrderItems { get; set; }
        public List<FnPaymentVoucherItem> PaymentItems { get; set; }
    }

    /// <summary>
    /// 采购付款单登记
    /// </summary>
    /// <remarks>
    /// 2016-03-24 杨云奕 添加
    /// </remarks>
    public class FnPurchasePaymentOrder
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 采购付款单编号
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 采购单系统编号
        /// </summary>
        public int ProcurementSysNo { get; set; }
        /// <summary>
        /// 采购单编号
        /// </summary>
        public string ProcurementNumber { get; set; }
        /// <summary>
        /// 制单时间
        /// </summary>
        public DateTime CreateTime { get; set; }
       /// <summary>
       /// 制单人
       /// </summary>
        public int CreateBy { get; set; }
        /// <summary>
        /// 制单人姓名
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 厂家编号
        /// </summary>
        public string ManufacturerNumber { get; set; }
        /// <summary>
        /// 审核人id
        /// </summary>
        public int AuditorBy { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditorTime { get; set; }
        /// <summary>
        /// 财务人id
        /// </summary>
        public int FinancialBy { get;set; }
        /// <summary>
        /// 财务时间
        /// </summary>
        public DateTime FinancialTime { get; set; }
        /// <summary>
        /// 记录付款单编号
        /// </summary>
        public int PVSysNo { get; set; }

        /// <summary>
        /// 获取总金额数据
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
