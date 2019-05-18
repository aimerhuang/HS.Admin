using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// 用于APP发票对象
    /// </summary>
    /// <remarks>2013-08-01 周唐炬 创建</remarks>
    public class AppFnInvoice
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 发票类型系统编号
        /// </summary>
        public int InvoiceTypeSysNo { get; set; }
        /// <summary>
        /// 发票代码
        /// </summary>
        public string InvoiceCode { get; set; }
        /// <summary>
        /// 发票号码
        /// </summary>
        public string InvoiceNo { get; set; }
        /// <summary>
        /// 发票金额
        /// </summary>
        public decimal InvoiceAmount { get; set; }
        /// <summary>
        /// 发票备注
        /// </summary>
        public string InvoiceRemarks { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string InvoiceTitle { get; set; }
        /// <summary>
        /// 状态:待开票(10),已开票(20),已取回(30),作废(-10)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateDate { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public string TransactionSysNo { get; set; }

        /// <summary>
        /// 发票类型名称
        /// </summary>
        public string InvoiceTypeName { get; set; }
    }
}
