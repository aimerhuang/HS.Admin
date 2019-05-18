using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 简单流量分析对象
    /// <remarks>2013-12-13 邵斌 创建</remarks>
    /// </summary>
    public class CBMkTrafficStatisticsSampleAnalysis
    {
        /// <summary>
        /// 流量
        /// </summary>
        public double PV { get; set; }

        /// <summary>
        /// IP数
        /// </summary>
        public double IP { get; set; }

        /// <summary>
        /// 评价访问页面数
        /// </summary>
        public decimal AverageViewPages { get; set; }

        /// <summary>
        /// 评价停留时间
        /// </summary>
        public double AverageStayTime { get; set; }

        /// <summary>
        /// 老用户回访率
        /// </summary>
        public decimal OldCustomerReviewRate { get; set; }
    }
}
