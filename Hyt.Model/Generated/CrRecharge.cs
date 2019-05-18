using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 充值记录
    /// </summary>
    /// <remarks>
    /// 2016-06-06 周 创建
    /// </remarks>
    [Serializable]
    public partial class CrRecharge
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 内部订单号
        /// </summary>
        [Description("内部订单号")]
        public string TradeNo { get; set; }
        /// <summary>
        /// 外部订单号
        /// </summary>
        [Description("外部订单号")]
        public string OutTradeNo { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        [Description("会员编号")]
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        [Description("充值金额")]
        public decimal ReAmount { get; set; }
        /// <summary>
        /// 支付方式名称
        /// </summary>
        [Description("支付方式名称")]
        public string RePaymentName { get; set; }
        /// <summary>
        /// 支付方式ID
        /// </summary>
        [Description("支付方式ID")]
        public int RePaymentId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime ReAddTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string ReMark { get; set; }
        /// <summary>
        /// 状态0未支付1已支付
        /// </summary>
        [Description("状态")]
        public int State { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        public int IsDelete { get; set; }
    }
}