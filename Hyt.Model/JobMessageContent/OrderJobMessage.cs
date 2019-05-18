using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.JobMessageContent
{
    /// <summary>
    /// 任务消息 (新建订单消息)
    /// </summary>
    /// <remarks>2015-1-21 杨浩 创建</remarks>
    public class OrderJobMessage:JobMessage
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 客服编号
        /// </summary>
        public int? AssignTo { get; set; }
    }
}
