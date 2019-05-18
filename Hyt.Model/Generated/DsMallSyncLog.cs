using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    /// <summary>
    /// 升舱商城同步日志表
    /// </summary>
    /// <remarks>
    /// 2014-07-23 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class DsMallSyncLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 同步类型:发货(10)
        /// </summary>
        [Description("同步类型:发货(10) 订单(20)")]
        public int SyncType { get; set; }
        /// <summary>
        /// 同步耗时(毫秒)
        /// </summary>
        [Description("同步耗时(毫秒)")]
        public int ElapsedTime { get; set; }
        /// <summary>
        /// 同步消息
        /// </summary>
        [Description("同步消息")]
        public string Message { get; set; }
        /// <summary>
        /// 同步数据
        /// </summary>
        [Description("同步数据")]
        public string Data { get; set; }
        /// <summary>
        /// 同步次数
        /// </summary>
        [Description("同步次数")]
        public int SyncNumber { get; set; }
        /// <summary>
        /// 最后同步时间
        /// </summary>
        [Description("最后同步时间")]
        public DateTime LastSyncTime { get; set; }
        /// <summary>
        /// 状态:成功(10),失败(20),作废(-10)
        /// </summary>
        [Description("状态:成功(10),失败(20),作废(-10)")]
        public int Status { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }
    }

    #region 同步商城订单 
    /// <summary>
    /// 2017/8/31 吴琨 创建
    /// </summary>
    public class SynchroMallSyncLog
    {
        /// <summary>
        /// 商城订单号
        /// </summary>
        [Description("商城订单号")]
        public string 商城订单号 { get; set; }


        /// <summary>
        /// 商城编号
        /// </summary>
        [Description("商城编号")]
        public int 商城编号 { get; set; }
    }
    #endregion
}
