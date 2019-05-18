using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 升舱订单明细关联(扩展
    /// </summary>
    /// 2013-09-09 朱成果 创建
    public class CBDsOrderItemAssociation : DsOrderItemAssociation
    {
        /// <summary>
        /// 商城订单明细
        /// </summary>
        public SoOrderItem HytSoOrderItem { get; set; }
     
    }
}
