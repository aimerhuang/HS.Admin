using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 大宗采购扩展属性类
    /// </summary>
    /// <remarks>
    /// 2013-07-02 杨晗 创建
    /// </remarks>
    public class CBCrBulkPurchase : CrBulkPurchase
    {
        /// <summary>
        /// 处理人
        /// </summary>
        /// <remarks>
        /// 2013-07-02 杨晗 创建
        /// </remarks>
        public string Handler { get; set; }
    }
}
