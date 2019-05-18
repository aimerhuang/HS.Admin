using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 用于导出配送单到Excel
    /// </summary>
    /// <remarks>2013-07-29 沈强 创建</remarks>
    public class DeliveryExportExcel
    {
        /// <summary>
        /// 配送单系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 配送员姓名（或快递名称）
        /// </summary>
        public string DeliveryManName { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreatedName { get; set; }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        public string CreatedDate { get; set; }

        /// <summary>
        /// 预付单量[已付款单量]
        /// </summary>
        public int PaidNoteCount { get; set; }

        /// <summary>
        /// 预付金额[已付款金额]
        /// </summary>
        public string PaidAmount { get; set; }

        /// <summary>
        /// 到付单量[货到付款单量]
        /// </summary>
        public int CODNoteCount { get; set; }

        /// <summary>
        /// 到付金额[货到付款金额]
        /// </summary>
        public string CODAmount { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
    }
}
