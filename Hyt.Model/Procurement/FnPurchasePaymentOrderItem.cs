using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    public class CBFnPurchasePaymentOrderItem : FnPurchasePaymentOrderItem
    { 
        
    }
    /// <summary>
    /// 采购支付单明细
    /// </summary>
    public class FnPurchasePaymentOrderItem
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 采购支付单主表
        /// </summary>
        public int PSysNo { get; set; }
        /// <summary>
        /// 厂家生产单编号
        /// </summary>
        public int PointOrderSysNo { get; set; }
        /// <summary>
        /// 付款单明细金额
        /// </summary>
        public int PaymentVoucherItemSysNo { get; set; }

        public decimal PaymentAmount { get; set; }

        public string CompanyName { get; set; }
        public string PayBankIDCard { get; set; }
        public string PayBankName { get; set; }

        public int PVSysNo { get; set; }

        public string ManufacturerNumber { get; set; }
        public string ManufacturerName { get; set; }
    }
}
