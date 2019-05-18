using System;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 会员分销商申请表
    /// </summary>
    /// <remarks>
    /// 2016-4-8 刘伟豪 创建
    /// </remarks>
    [Serializable]
    public partial class CrDealerApply
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 客户系统编号
        /// </summary>
        [Description("客户系统编号")]
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 状态：10提交，20通过，-10拒绝
        /// </summary>
        [Description("状态：10提交，20通过，-10拒绝")]
        public int Status { get; set; }

        /// <summary>
        /// 申请日期
        /// </summary>
        [Description("申请日期")]
        public DateTime AppliedDate { get; set; }

        /// <summary>
        /// 审核日期
        /// </summary>
        [Description("审核日期")]
        public DateTime? AuditedDate { get; set; }
    }
}