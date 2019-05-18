using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 分销商特殊价格实体
    /// </summary>
    /// <remarks>2013-09-04 周瑜 创建</remarks>
    public class CBDsSpecialPrice : DsSpecialPrice
    {
        /// <summary>
        /// ERP编码
        /// </summary>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        public string ErpCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        public string ProductName { get; set; }

        /// <summary>
        /// 分销商名称
        /// </summary>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        public string DealerName { get; set; }
    }
}
