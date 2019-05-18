using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 会员余额支付记录
    /// </summary>
    /// <remarks>
    /// 2016-06-06 周 创建
    /// </remarks>
    [Serializable]
    public partial class CrBalancePayOrderLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        [Description("会员ID")]
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [Description("订单编号")]
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        [Description("支付方式")]
        public int PayType { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        [Description("支付金额")]
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 会员账户剩余可用余额
        /// </summary>
        [Description("会员账户剩余可用余额")]
        public decimal MemberBalance { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        public int State { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        [Description("支付时间")]
        public DateTime PayTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        public int IsDelete { get; set; }

    }
}
