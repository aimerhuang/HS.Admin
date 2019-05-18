using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 退换货扣除返利
    /// </summary>
    /// <remarks>2016-1-5 杨浩 创建</remarks>
    public class CBReurnDeductRebates
    {
        /// <summary>
        /// 返利日志编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 退换货扣除返利
        /// </summary>
        public decimal DeductRebates { get; set; }
        /// <summary>
        /// 退换货状态
        /// </summary>
        public int ReturnStatus { get; set; }
        /// <summary>
        /// 退换货主表编号
        /// </summary>
        public int ReturnSysNo { get; set; }
    }
}
