using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.SMS
{
    /// <summary>
    /// 短信执行状态
    /// </summary>
    /// <remarks>2013-6-26 杨浩 添加</remarks>
    public enum SmsResultStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 失败
        /// </summary>
        Failue
    }

    /// <summary>
    /// 短信执行结果
    /// </summary>
    /// <remarks>2013-6-26 杨浩 添加</remarks>
    public class SmsResult
    {
        /// <summary>
        /// 执行结果
        /// </summary>
        public SmsResultStatus Status { get; set; }

        /// <summary>
        /// 执行行数
        /// </summary>
        public int RowCount { get; set; }

    }
}
