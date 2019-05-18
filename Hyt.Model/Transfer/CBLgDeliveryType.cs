using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 配送方式组合实体
    /// </summary>
    /// <remarks>
    /// 2013-07-03 郑荣华 创建
    /// </remarks>
    
    public class CBLgDeliveryType : LgDeliveryType
    {

        /// <summary>
        /// 父级名称 add
        /// </summary>
        public string ParentName { get; set; }
        public int Count { get; set; }

    }
}
