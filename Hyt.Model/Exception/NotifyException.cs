using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Exception
{
    /// <summary>
    /// 自定义通知类异常信息
    /// 异常消息需要用户看到
    /// </summary>
    /// <remarks>2013-10-31 黄波 创建</remarks>
    public sealed class NotifyException : System.Exception
    {
        /// <summary>
        /// 使用指定的消息初始化异常实例
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <remarks>2013-10-31 黄波 创建</remarks>
        public NotifyException(string message)
            : base(message)
        {

        }
    }
}
