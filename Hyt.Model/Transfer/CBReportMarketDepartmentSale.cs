using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 升舱明细
    /// </summary>
    /// <remarks>2013-9-16 黄伟 创建</remarks>
    public class CBReportMarketDepartmentSale : ReportMarketDepartmentSale
    {

        /// <summary>
        /// 发货开始日期,用于筛选查找
        /// </summary>
        public  DateTime? BeginDate { get; set; }

        /// <summary>
        /// 发货结束日期,用于筛选查找
        /// </summary>
        public DateTime? EndDate { get; set; }

    }
}
