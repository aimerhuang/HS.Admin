using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    public class CBWhInventoryDetail : WhInventoryDetail
    {
        public string ProductName { get; set; }
        public string ProductBarcode { get; set; }
        public string StockNumber { get; set; }
        public string WarePostion { get; set; }
        public string Category { get; set; }
        public string Price { get; set; }
    }
}
