using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Model
{
    /// <summary>
    /// WhStockIn实体类
    /// </summary>
    /// <remarks>2013-06-08 周唐炬 创建</remarks>
    public partial class WhStockIn : BaseEntity
    {
        /// <summary>
        /// 入库单明细-入库商品 
        /// </summary>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public List<WhStockInItem> ItemList { get; set; }

        /// <summary>
        /// 发票系统编号
        /// </summary>
        public int? InvoiceSysNo { get; set; }
    }
}
