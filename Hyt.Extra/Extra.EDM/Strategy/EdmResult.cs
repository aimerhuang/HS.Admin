using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.EDM
{

    /// <summary>
    /// EDM发送结果状态
    /// </summary>
    /// <remarks>
    /// 2013/4/12 何方 创建
    /// </remarks>
    public enum EdmResultStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success=1,

        /// <summary>
        /// 失败
        /// </summary>
        Failue=0
    }

    /// <summary>
    /// EDM发送结果
    /// </summary>
    /// <remarks>
    /// 2013/4/12 何方 创建
    /// </remarks>
    public class EdmResult
    {
        /// <summary>
        /// 执行结果
        /// </summary>
        public EdmResultStatus Status { get; set; }

        /// <summary>
        /// 返回结果详细信息
        /// </summary>
        public string Message { get; set; }

    }
}
