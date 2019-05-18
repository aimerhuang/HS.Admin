using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Exception
{
    /// <summary>
    /// 购物车对象为空
    /// </summary>
    /// <remarks>2013-10-31 黄波 创建</remarks>
    public sealed class ShoppingCartNullException : System.Exception
    {
        /// <summary>
        /// 初始化异常实例
        /// </summary>
        /// <remarks>2013-10-31 黄波 创建</remarks>
        public ShoppingCartNullException() : base() { }

        /// <summary>
        /// 异常信息
        /// </summary>
        public override string Message
        {
            get
            {
                return "购物车没有商品!";
            }
        }
    }
}