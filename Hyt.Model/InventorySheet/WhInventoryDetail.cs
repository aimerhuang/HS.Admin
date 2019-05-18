using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.InventorySheet
{
    /// <summary>
    /// 盘点单明细实体
    /// </summary>
    public class WhInventoryDetail : WhInventory
    {
        /// <summary>
        /// 盘点单商品明细
        /// </summary>
        public List<WhInventoryProductDetail> dataList { get; set; }


    }
}
