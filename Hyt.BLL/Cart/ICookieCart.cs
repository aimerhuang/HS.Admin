using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Cart
{
    /// <summary>
    /// 本地购物车转换
    /// </summary>
    /// <remarks>2013-10-16 黄波 创建</remarks>
    interface ICookieCart
    {
        /// <summary>
        /// 将本地购物车存到客户数据库购物车中
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        void ToDatabase(int customerSysNo);
    }
}
