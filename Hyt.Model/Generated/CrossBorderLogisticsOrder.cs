using System;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 陈海裕创建
    /// </summary>
    [Serializable]
    public class CrossBorderLogisticsOrder
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 销售单系统编号
        /// </summary>
        [Description("销售单系统编号")]
        public int SoOrderSysNo { get; set; }
        /// <summary>
        /// 推送物流返回订单号
        /// </summary>
        [Description("推送物流返回订单号")]
        public string LogisticsOrderId { get; set; }
        /// <summary>
        /// 物流代码
        /// </summary>
        [Description("物流代码")]
        public int LogisticsCode { get; set; }
        /// <summary>
        /// 物流运单号
        /// </summary>
        [Description("物流运单号")]
        public string ExpressNo { get; set; }
    }
}
