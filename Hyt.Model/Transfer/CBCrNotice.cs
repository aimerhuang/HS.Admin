using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 商品信息通知扩展类
    /// </summary>
    /// <remarks>2014-01-24 苟治国 添加</remarks>
    public class CBCrNotice : CrNotice
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品状态
        /// </summary>
        public int ProductStatus { get; set; }
    }
}
