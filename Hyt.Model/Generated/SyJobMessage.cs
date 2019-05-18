using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 任务消息
    /// </summary>
    /// <remarks>2015-1-21 杨浩 创建</remarks>
    public class SyJobMessage
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public int MessageType { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
