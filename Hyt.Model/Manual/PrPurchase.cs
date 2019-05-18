using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 采购单
    /// </summary>
    /// <remarks>2016-6-17 杨浩 创建</remarks>
    public partial class PrPurchase:BaseEntity
    {
        /// <summary>
        /// 采购单详情列表
        /// </summary>
        public IList<PrPurchaseDetails> PurchaseDetails { get; set; }
    }
}
