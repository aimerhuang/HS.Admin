using System;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 订单其他费用（信营：太平洋保险）
    /// </summary>
    /// <remarks>
    /// 2016-09-28 罗远康 创建
    /// </remarks>
    [Serializable]
    public partial class SoOrderOtherExpenses
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [Description("订单编号")]
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 费用类型：0太平洋保险
        /// </summary>
        [Description("费用类型：0太平洋保险")]
        public int ExpensesType { get; set; }
        /// <summary>
        /// 费用金额
        /// </summary>
        [Description("费用金额")]
        public decimal ExpensesAmount { get; set; }
    }
}
