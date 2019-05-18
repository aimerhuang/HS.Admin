using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 会员可用积分日志查询筛选字段
    /// </summary>
    /// <remarks>
    /// 2013-12-01 苟治国 创建
    /// </remarks>
    public class ParaCrAvailablePointLogFilter
    {
        /// <summary>
        /// 客户编号
        /// </summary>
        public int? CustomerSysNo { get; set; }

        /// <summary>
        /// 创建日(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 结束日(止)
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
