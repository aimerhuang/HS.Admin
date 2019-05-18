using System;
using System.ComponentModel;

namespace Hyt.Model
{
    public class DBAtAllocation : AtAllocation
    {
        public string OutWarehousCode { get; set; }
        public string EnterWarehousCode { get; set; }
    }
    public class AtAllocation
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 调出仓库系统编号
        /// </summary>
        [Description("调出仓库系统编号")]
        public int OutWarehouseSysNo { get; set; }

        /// <summary>
        /// 调入仓库系统编号
        /// </summary>
        [Description("调入仓库系统编号")]
        public int EnterWarehouseSysNo { get; set; }

        /// <summary>
        /// 调拨编号
        /// </summary>
        [Description("调拨编号")]
        public string AllocationCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        [Description("审核人")]
        public int CheckUserSysNo { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Description("审核时间")]
        public DateTime CheckDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        public int Status { get; set; }
    }
}
