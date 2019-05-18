using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 配送单维护查询实体类
    /// </summary>
    /// <remarks>
    /// 2013-06-17 14:46 沈强 创建
    /// </remarks>
    public class CBLgDelivery
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        ///// <summary>
        ///// 事务编号
        ///// </summary>
        //public string TransactionSysNo { get; set; }

        /// <summary>
        /// 仓库系统编号
        /// </summary>
        public int StockSysNo { get; set; }

        /// <summary>
        /// 当前用户有权限的仓库系统编号-huangwei 11.27
        /// </summary>
        public List<int> StockSysNos { get; set; }

        /// <summary>
        /// 配送员系统编号
        /// </summary>
        public int DeliveryUserSysNo { get; set; }

        /// <summary>
        /// 配送员姓名
        /// </summary>
        public string DeliveryManName { get; set; }

        /// <summary>
        /// 预付金额
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 预付单量
        /// </summary>
        public int PaidNoteCount { get; set; }

        /// <summary>
        /// 到付金额
        /// </summary>
        public decimal CODAmount { get; set; }

        /// <summary>
        /// 到付单量
        /// </summary>
        public int CODNoteCount { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否在额度不够时强制放行
        /// </summary>
        public int IsEnforceAllow { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        public int DeliveryTypeSysNo { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreatedName { get; set; }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }

        ///// <summary>
        ///// 最后更新人
        ///// </summary>
        //public int LastUpdateBy { get; set; }

        ///// <summary>
        ///// 最后更新时间
        ///// </summary>
        //public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// 出库单号（仅用于查询）
        /// </summary>
        public string StockOutNo { get; set; }

        /// <summary>
        /// 任务创建开始时间（仅用于查询）
        /// </summary>
        public string CreatedStartDate { get; set; }

        /// <summary>
        /// 任务创建结束时间（仅用于查询）
        /// </summary>
        public string CreatedEndDate { get; set; }

        /// <summary>
        /// 配送单是否可以作废标志（只有为 0 时配送单可以作废）
        /// </summary>
        public int IsCancel { get; set; }

        /// <summary>
        /// 配送方式名称
        /// </summary>
        public string DeliveryTypeName { get; set; }

        /// <summary>
        /// 是否为三方快递 是（1），否（0）
        /// </summary>
        public string IsThirdPartyExpress { get; set; }

        /// <summary>
        /// 用于分页查询，无意义
        /// </summary>
        public int FLUENTDATA_ROWNUMBER { get; set; }
    }
}
