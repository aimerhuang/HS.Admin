using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    public class CBDsTakeStockItem:DsTakeStockItem
    {
        public decimal BasePrice { get; set; }
    }
    /// <summary>
    /// 所判断不同的商品明细
    /// </summary>
    public class DsTakeStockItem
    {
        public int SysNo { get; set; }
        public int ProductSysNo { get; set; }
        public string ProductBarCode { get; set; }
        public string ProductName { get; set; }
        public int ProOldNum { get; set; }
        public int ProNowNum { get; set; }
        public string ProDis { get; set; }
        public int PSysNo { get; set; }
    }
}
