using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 会员余额
    /// </summary>
    /// <remarks>
    /// 2016-06-06 周 创建
    /// </remarks>
    [Serializable]
    public partial class CrAccountBalance
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
        /// 可用余额
        /// </summary>
        [Description("可用余额")]
        public decimal AvailableBalance { get; set; }
        /// <summary>
        /// 冻结余额
        /// </summary>
        [Description("冻结余额")]
        public decimal FrozenBalance { get; set; }
        /// <summary>
        /// 总余额
        /// </summary>
        [Description("总余额")]
        public decimal TolBlance { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Description("描述")]
        public string Remark { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        public int State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime AddTime { get; set; }

    }

}