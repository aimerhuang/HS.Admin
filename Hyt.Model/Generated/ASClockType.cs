using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    /// <summary>
    /// 签到类型
    /// </summary>
    /// <remarks>
    /// 2016-05-26 周海鹏 创建
    /// </remarks>
    [Serializable]
    public partial class ASClockType
    {
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 签到类型名称
        /// </summary>
        [Description("签到类型名称")]
        public string TypeName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        public int TypeSort { get; set; }
        /// <summary>
        /// 状态:启用(1),禁用(0)
        /// </summary>
        [Description("状态:启用(1),禁用(0)")]
        public int TypeState { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        [Description("时间")]
        public DateTime TypeTime { get; set; }
        /// <summary>
        /// 状态:启用(1),禁用(0)
        /// </summary>
        [Description("状态:启用(1),禁用(0)")]
        public string Remark { get; set; }
        /// <summary>
        /// 状态:否(0),是(1)
        /// </summary>
        [Description("是否删除:否(0),是(1)")]
        public int IsDelete { get; set; }
    }
}
