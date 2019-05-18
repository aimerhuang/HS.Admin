using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 退换货日志扩展
    /// </summary>
    /// <remarks>
    /// 2013-07-13 朱成果 创建
    /// </remarks>
    public class CBRcReturnLog : RcReturnLog
    {
        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string UserName { get; set; }
    }
}
