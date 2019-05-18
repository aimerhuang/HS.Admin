using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 报表汇总显示实体(目前用于销售明细/退换货明细)
    /// </summary>
    ///<remarks>2013-12-6 黄伟 创建</remarks>
    public class CBReportFooter:BaseEntity
    {
        public int 数量 { get; set; }

        public decimal 优惠 { get; set; }

        public decimal 销售金额 { get; set; }

        public decimal 实收金额 { get; set; }
    }
}
