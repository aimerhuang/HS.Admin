using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model
{
    /// <summary>
    /// ERP 单据来源
    /// </summary>
    /// <remarks>2015/2/11 10:45:36 朱成果 创建</remarks>
    public class ErpBillSource
    {
        public enum 单据来源
        {
            未知=0,
            配送=1,
            结算签收=2,
            结算未送达=3,
            结算拒收=4,
            退换货=5,
            // 结算部分签收=6,
            调货=7
           
        }
    }
}
