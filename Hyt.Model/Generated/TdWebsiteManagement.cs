using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 网站购物下单管理状态
    /// </summary>
    /// <remarks>
    /// 2016-07-1 周 创建
    /// </remarks>
    [Serializable]
    public partial class TdWebsiteManagement
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 是否正常下单0正常1停用2延迟配送
        /// </summary>
        [Description("是否正常下单0正常1停用2延迟配送")]
        public int WebsiteState { get; set; }
        /// <summary>
        /// 停用状态
        /// </summary>
        [Description("停用状态")]
        public int WebsiteStateSysno { get; set; }
        /// <summary>
        /// 延迟配送时间（分钟）
        /// </summary>
        [Description("延迟配送时间（分钟）")]
        public int DelayLongTime { get; set; }
        /// <summary>
        /// 延迟配送原因
        /// </summary>
        [Description("延迟配送原因")]
        public string DelayReason { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>
        [Description("店铺ID")]
        public int WarehouseSysNo { get; set; }
    }
}
