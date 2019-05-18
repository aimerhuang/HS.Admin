using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 关注商品扩展实体
    /// </summary>
    /// <remarks>
    /// 2013-08-26 郑荣华 创建
    /// </remarks>
    public class CBCrFavorites : CrFavorites
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductImage { get; set; }

    }
}
