using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 业务员绩效
    /// </summary>
    /// <remarks>2014-1-15 沈强 创建</remarks>
    public class ParaBusinessManPerformance : RP_绩效_业务员
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 是否自营
        /// </summary>
        /// <remarks>2014-08-27 余勇 创建</remarks>
        public int? IsSelfSupport { get; set;}

    }
}
