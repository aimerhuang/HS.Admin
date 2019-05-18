using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Exception
{
    /// <summary>
    /// 惠源币余额不足
    /// </summary>
    /// <remarks>2013-10-31 黄波 创建</remarks>
    public sealed class NotEnoughExperienceCoinException : System.Exception
    {
        /// <summary>
        /// 初始化异常实例
        /// </summary>
        /// <remarks>2013-10-31 黄波 创建</remarks>
        public NotEnoughExperienceCoinException()
            : base()
        {

        }

        /// <summary>
        /// 异常消息
        /// </summary>
        public override string Message
        {
            get
            {
                return "惠源币余额不足";
            }
        }
    }
}
