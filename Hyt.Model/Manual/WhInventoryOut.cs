using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Model
{
    /// <summary>
    /// WhInventoryOut实体类
    /// </summary>
    /// <remarks>2013-06-08 王耀发 创建</remarks>
    public partial class WhInventoryOut : BaseEntity
    {
        /// <summary>
        /// 出库单明细-出库商品 
        /// </summary>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public List<WhInventoryOutItem> ItemList { get; set; }
    }
}
