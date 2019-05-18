using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 经验积分日志查询筛选字段
    /// </summary>
    /// <remarks>
    /// 2013-11-1 苟治国 创建
    /// </remarks>
    public class ParaCrExperiencePointLogFilter
    {
        /// <summary>
        /// 客户编号
        /// </summary>
        public int? CustomerSysNo { get; set; }

        /// <summary>
        /// 查询类型
        /// </summary>
        public int? Type { get; set; }

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
