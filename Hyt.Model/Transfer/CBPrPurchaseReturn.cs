using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 采购单查询实体
    /// </summary>
    /// <remarks>2016-6-15 杨浩 创建</remarks>
    public class CBPrPurchaseReturn : PrPurchaseReturn
    {
        /// <summary>
        /// 仓库后台显示名称
        /// </summary>
        public string BackWarehouseName { get; set; }

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseCode { get; set; }
    }
}
