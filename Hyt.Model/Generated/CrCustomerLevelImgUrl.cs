using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Hyt.Model
{
    public partial class CrCustomerLevelImgUrl
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 会员等级编号
        /// </summary>
        [Description("会员等级编号")]
        public int CrCustomerLevelSysNo { get; set; }
        /// <summary>
        /// 会员等级图标
        /// </summary>
        [Description("会员等级图标")]
        public string CustomerLevelImgUrl { get; set; }

    }
}
