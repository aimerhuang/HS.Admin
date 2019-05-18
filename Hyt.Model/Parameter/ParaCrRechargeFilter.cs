using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Transfer;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 充值记录查询参数
    /// </summary>
    /// <remarks> 2016-08-17 刘伟豪 创建 </remarks>
    public class ParaCrRechargeFilter : CBCrRecharge
    {
        /// <summary>
        /// 创建开始时间范围
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 创建结束时间范围
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 分页查询id
        /// </summary>
        public int? id { get; set; }

        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// 模糊查询
        /// </summary>
        public string KeyWord { get; set; }
    }
}