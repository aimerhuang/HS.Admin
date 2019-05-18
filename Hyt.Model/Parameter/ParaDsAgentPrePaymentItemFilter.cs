using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 代理商预存款来往明细查询条件类
    /// </summary>
    /// <remarks> 2016-04-18 刘伟豪 创建 </remarks>
    public class ParaDsAgentPrePaymentItemFilter
    {
        /// <summary>
        /// 分页页码
        /// </summary>
        public int? id { get; set; }

        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// 代理商系统编号
        /// </summary>
        public int? SysNo { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 预存款明细单据来源:预存款(10),冻结返回(20),退换货(30),订单消
        /// </summary>
        public int? Source { get; set; }

        /// <summary>
        /// 状态
        /// 分销商往来账明细状态:枚举 DistributionStatus.预存款明细状态
        /// </summary>
        public int? Status { get; set; }
    }
}