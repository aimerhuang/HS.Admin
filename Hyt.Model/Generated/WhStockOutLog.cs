using System;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 出库单详细日志
    /// </summary>
    /// <remarks>2014-12-18 杨浩 创建</remarks>
    [Serializable]
    public partial class WhStockOutLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 出库单编号
        /// </summary>
        [Description("出库单编号")]
        public int WhStockOutSysNo { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        [Description("日志内容")]
        public string LogContent { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [Description("操作人")]
        public string Operator { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [Description("操作时间")]
        public DateTime OperateDate { get; set; }
    }
}
