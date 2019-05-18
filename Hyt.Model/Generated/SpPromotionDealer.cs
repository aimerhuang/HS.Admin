using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 签到类型
    /// </summary>
    /// <remarks>
    /// 2016-05-26 周海鹏 创建
    /// </remarks>
    [Serializable]
    public partial class SpPromotionDealer
    {
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 促销编号
        /// </summary>
        [Description("促销编号")]
        public int PromotionSysNo { get; set; }
        /// <summary>
        /// 分销商编号
        /// </summary>
        [Description("分销商编号")]
        public int DealerSysNo { get; set; }

    }
}
