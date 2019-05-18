using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 办事处绩效
    /// </summary>
    ///  <remarks>2014-1-15 沈强 创建</remarks>
    public class ParaOfficePerformance : rp_绩效_办事处
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? EndDate { get; set; }

    }
}
