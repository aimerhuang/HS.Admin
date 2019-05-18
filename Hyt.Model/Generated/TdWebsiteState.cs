using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 网站停止购物下单状态
    /// </summary>
    /// <remarks>
    /// 2016-07-1 周 创建
    /// </remarks>
    [Serializable]
    public partial class TdWebsiteState
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Description("开始时间")]
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Description("结束时间")]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 停用备注
        /// </summary>
        [Description("停用备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 是否每天重复时间段使用此状态0否1是
        /// </summary>
        [Description("是否每天重复时间段使用此状态0否1是")]
        public int IfRepeatDate { get; set; }
        /// <summary>
        /// 状态0使用1停用
        /// </summary>
        [Description("状态0使用1停用")]
        public int Status { get; set; }
        /// <summary>
        /// 是否删除0否1是
        /// </summary>
        [Description("是否删除0否1是")]
        public int Isdelete { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        [Description("仓库ID")]
        public int WarehouseSysNo { get; set; }
    }
}
