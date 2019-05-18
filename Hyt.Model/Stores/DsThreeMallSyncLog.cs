using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Stores
{
    /// <summary>
    /// 三方商城同步日志
    /// </summary>
    /// <remarks>2016-7-8 杨浩 创建</remarks>
    [Serializable]
    public class DsThreeMallSyncLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }  
        /// <summary>
        /// 商城编号
        /// </summary>
        public int MallSysNo { get; set; }
        /// <summary>
        /// 同步类型
        /// </summary>
        public int SyncType { get; set; }

        /// <summary>
        /// 最后同步时间
        /// </summary>
        public DateTime LastSyncTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
