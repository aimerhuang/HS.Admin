using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Exception
{
    /// <summary>
    /// 找不到匹配的用户
    /// </summary>
    /// <remarks>2013-10-31 黄波 创建</remarks>
    public sealed class UserNotMatchException : System.Exception
    {
        private int _customerSysNo;

        /// <summary>
        /// 使用指定的用户编号初始化异常实例
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <remarks>2013-10-31 黄波 创建</remarks>
        public UserNotMatchException(int customerSysNo)
        {
            _customerSysNo = customerSysNo;
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        public override string Message
        {
            get
            {
                return string.Format("{0}用户未找到!",_customerSysNo);
            }
        }
    }
}
