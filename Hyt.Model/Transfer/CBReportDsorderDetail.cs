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
    public class CBReportDsorderDetail : ReportDsorderDetail
    {

        /// <summary>
        /// 订单开始日期,用于筛选查找
        /// </summary>
        public  DateTime? OrderBeginDate { get; set; }

        /// <summary>
        /// 订单结束日期,用于筛选查找
        /// </summary>
        public DateTime? OrderEndDate { get; set; }

        /// <summary>
        /// 商城类型,用于筛选查找
        /// </summary>
        public int? MallType { get; set; }
    }
}
