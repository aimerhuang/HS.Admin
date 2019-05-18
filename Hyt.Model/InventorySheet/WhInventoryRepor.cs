using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.InventorySheet
{
    /// <summary>
    /// 盘点报告单
    /// </summary>
    public class WhInventoryRepor
    {
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }


        /// 盘点单系统编号(关联盘点单)
        /// </summary>
        [Description("盘点单系统编号(关联盘点单)")]
        public string WhInventoryCode { get; set; }


        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }


        /// 报告类别
        /// </summary>
        [Description("报告类别")]
        public int ReportType { get; set; }


        /// 打印次数
        /// </summary>
        [Description("打印次数")]
        public int PrintCount { get; set; }


        /// 盘点商品仓库编号
        /// </summary>
        [Description("盘点商品仓库编号")]
        public string WarehouseCode { get; set; }


        /// 盘点仓库名称
        /// </summary>
        [Description("盘点仓库名称")]
        public string WarehouseName { get; set; }


        /// 生成报告单日期
        /// </summary>
        [Description("生成报告单日期")]
        public DateTime? Time { get; set; }


        /// 制单机构
        /// </summary>
        [Description("制单机构")]
        public string Make { get; set; }



        /// 审核
        /// </summary>
        [Description("审核")]
        public string Audit { get; set; }


        /// 记账
        /// </summary>
        [Description("记账")]
        public string Tally { get; set; }


        /// 保管人名称
        /// </summary>
        [Description("保管人名称")]
        public string CustodySysNo { get; set; }


        /// 负责人名称
        /// </summary>
        [Description("负责人名称")]
        public string HeadSysNo { get; set; }


        /// 经办人名称
        /// </summary>
        [Description("经办人名称")]
        public string AgentSysNo { get; set; }



        /// 盈亏状态
        /// </summary>
        [Description("盈亏状态")]
        public int YingKuiStatus { get; set; }


        /// 盘点报告单状态
        /// </summary>
        [Description("盘点报告单状态")]
        public int? Status { get; set; }

        

        /// 制单人系统编号
        /// </summary>
        [Description("制单人系统编号")]
        public int AddUser { get; set; }

        /// 审核日期
        /// </summary>
        [Description("审核日期")]
        public DateTime? AuditTime { get; set; }


        /// 盘点报告单创建日期
        /// </summary>
        [Description("盘点报告单创建日期")]
        public DateTime AddTime { get; set; }


        #region 扩展属性 

        /// 盘点报告单商品详情
        /// </summary>
        [Description("盘点报告单商品详情")]
        public List<WhIReporPrDetails> DataList { get; set; }

        /// <summary>
        /// 盘盈状态 
        /// </summary>
        public bool PanYingStatus { get; set; }

        /// <summary>
        /// 盘亏状态 
        /// </summary>
        public bool PanKuiStatus { get; set; }

        #endregion
    }
}
