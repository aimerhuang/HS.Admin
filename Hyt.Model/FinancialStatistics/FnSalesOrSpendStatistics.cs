using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.FinancialStatistics
{
    /// <summary>
    /// 统计表明细
    /// </summary>
    public class CBFnSalesOrSpendStatistics : FnSalesOrSpendStatistics
    {
        public string StatisticTypeName { get; set; }
    }
   /// <summary>
   /// 统计表销售和支出的统计表
   /// </summary>
    public  class FnSalesOrSpendStatistics
    {
        public int SysNo { get; set; }
        public int PSysNo { get; set; }
        public int PTSysNo { get; set; }
        public int PayType { get; set; }
        public int PayCode { get; set; }
        public string PayText { get; set; }
        public decimal PayAmount { get; set; }
    }
}
