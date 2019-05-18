using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 会员分销商申请表查询参数
    /// </summary>
    /// <remarks>2016-04-08 刘伟豪 创建</remarks>
    public class ParaCrDealerApplyFilter : CBCrDealerApply
    {
        /// <summary>
        /// 申请开始时间范围
        /// </summary>
        public DateTime? AppliedDateStartTime { get; set; }

        /// <summary>
        /// 申请结束时间范围
        /// </summary>
        public DateTime? AppliedDateEndTime { get; set; }

        /// <summary>
        /// 审核开始时间范围
        /// </summary>
        public DateTime? AuditedDateStartTime { get; set; }

        /// <summary>
        /// 审核结束时间范围
        /// </summary>
        public DateTime? AuditedDateEndTime { get; set; }

        /// <summary>
        /// 分页查询id
        /// </summary>
        public int? id { get; set; }

        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// 模糊查询关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 是否有查看所有的权限
        /// </summary>
        public bool CanSearchAll { get; set; }

        /// <summary>
        /// 当前账号所绑定的分销商
        /// </summary>
        public int BindDealerSysNo { get; set; }

        /// <summary>
        /// 技师绑定状态查询条件（重城）：1已绑定，0未绑定
        /// </summary>
        public int? BindStatus { get; set; }
    }
}