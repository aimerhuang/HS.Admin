using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 采购退货单
    /// </summary>
    /// <remarks>2016-6-17 王耀发 创建</remarks>
    public partial class PrPurchaseReturn:BaseEntity
    {
        /// <summary>
        /// 采购单详情列表
        /// </summary>
        public IList<PrPurchaseReturnDetails> PurchaseReturnDetails { get; set; }
    }
}

	