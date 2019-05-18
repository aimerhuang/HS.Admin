using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 打印模板组合实体
    /// </summary>
    /// <remarks>
    /// 2013-07-12 郑荣华 创建
    /// </remarks>
    public class CBLgDeliveryPrintTemplate : LgDeliveryPrintTemplate
    {
        /// <summary>
        /// 配送方式名称
        /// </summary>
        public string DeliveryTypeName { get; set; }
    }
}
