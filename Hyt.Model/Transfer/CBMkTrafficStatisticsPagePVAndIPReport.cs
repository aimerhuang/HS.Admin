using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 页面报告
    /// </summary>
    /// <remarks>2013-11-19 邵斌 创建</remarks>
    [Serializable]
    public class CBMkTrafficStatisticsPagePVAndIPReport
    {
        /// <summary>
        /// Url地址
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// PV总量
        /// </summary>
        public string TotalPV { get; set; }

        /// <summary>
        /// IP总量
        /// </summary>
        public string TotalIP { get; set; }
    }
}
