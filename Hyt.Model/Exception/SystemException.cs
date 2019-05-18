using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Exception
{
    /// <summary>
    /// 系统异常
    /// </summary>
    /// <remarks>2013-10-31 黄波 创建</remarks>
    public sealed class SystemException : System.Exception
    {
        /// <summary>
        /// 使用指定的消息初始化异常实例
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <remarks>2013-10-31 黄波 创建</remarks>
        public SystemException(string message)
            : base(message)
        {
        }
    }
}
