using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 任务池短信设置
    /// </summary>
    /// <remarks>
    /// 2014-8-5 陈俊创建
    /// </remarks>
    [Serializable]
    public class SyJobSmsConfig : BaseEntity
    {
        /// <summary>
        /// 字段编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 短信接收人编号
        /// </summary>
        public int ReceiveSysNo { get; set; }
        /// <summary>
        /// 最大未处理时间
        /// </summary>
        public int MaxDealTime { get; set; }
        /// <summary>
        /// 上次发送短信时间
        /// </summary>
        public DateTime LastSendTime { get; set; }
    }
}
