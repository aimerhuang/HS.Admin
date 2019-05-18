using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 分销商预存款往来账明细扩展
    /// </summary>
    /// <remarks>2013-09-11 周唐炬 创建</remarks>
    public class CBDsPrePaymentItem : DsPrePaymentItem
    {
        /// <summary>
        /// 分销商名称
        /// </summary>
        /// <remarks>2013-09-11 周唐炬 创建</remarks>
        public string DealerName { get; set; }

        /// <summary>
        /// 增加金额合计(ui显示)
        /// </summary>
        public decimal TotalIncreased { get; set; }

        /// <summary>
        /// 减少金额合计(ui显示)
        /// </summary>
        public decimal TotalDecreased { get; set; }

        /// <summary>
        /// 剩余金额合计(ui显示)
        /// </summary>
        public decimal TotalSurplus { get; set; }
    }
}
