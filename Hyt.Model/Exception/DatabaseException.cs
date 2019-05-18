using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Exception
{
    /// <summary>
    /// 数据库异常
    /// </summary>
    /// <remarks>2013-10-31 黄波 创建</remarks>
    public sealed class DatabaseException : System.Exception
    {
        /// <summary>
        /// 使用指定的消息初始化异常实例
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <remarks>2013-10-31 黄波 创建</remarks>
        public DatabaseException(string message)
            : base(message)
        {

        }
    }
}
