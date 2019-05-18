using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 借货单明细扩展
    /// </summary>
    /// <remarks>2014-01-13 ZTJ 添加注释</remarks>
    public class CBWhProductLendItem : WhProductLendItem
    {
        /// <summary>
        /// 信用价格
        /// </summary>
        public decimal Price { get; set; }
    }
}
