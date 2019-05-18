using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    /// <summary>
    /// 数据推送任务功能操作
    /// </summary>
    public class SyWSOrderToClientJob
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        public int DsSysNo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime PushTime { get; set; }
        /// <summary>
        /// 推送数据
        /// </summary>
        public string PushData { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}
