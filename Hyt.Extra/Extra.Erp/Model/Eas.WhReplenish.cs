using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model
{
    /// <summary>
    /// 返还单
    /// </summary>
    ///<remarks>2015-5-4 谭显锋 创建</remarks>
    public class WhReplenish
    {


        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 企业Eas编号
        /// </summary>
        public string EnterpriseCode { get; set; }

        /// <summary>
        /// 单据编号
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// 出库单号
        /// </summary>
        public int OutStockNo { get; set; }

        /// <summary>
        /// 状态（-10 作废,待提交10，待审核20，未通过30，已审核 40，已完成50）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatorSysNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        public int LastUpdatedBy { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdatedTime { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public int Auditor { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// 审核意见
        /// </summary>
        public string AuditRemarks { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public int WarehouseSysNo { get; set; }

        /// <summary>
        /// 仓库ERP编号
        /// </summary>
        public string WarehouseErpCpde { get; set; }

        /// <summary>
        /// 补货类型
        /// </summary>
        public 补货类型 type { get; set; }

      

    }
    /// <summary>
    /// 补货类型
    /// </summary>
    public enum 补货类型 : int
    {
        补货 = 999,
        补货退回补收 = 998,
        加盟商减应收=900,
        电商出库=930
    }
}
