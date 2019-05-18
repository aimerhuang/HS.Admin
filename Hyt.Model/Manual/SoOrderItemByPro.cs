using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Manual
{
    public class SoOrderItemByPro : SoOrderItem
    {
        public string GCode { get; set; }
        public string Hscode { get; set; }
        public string CiqGoodsNo { get; set; }
        public string Brand { get; set; }
        public string Spec { get; set; }
        public string Origin { get; set; }
        public string Qty { get; set; }
        public string QtyUnit { get; set; }
        public decimal DecPrice { get; set; }
        public decimal DecTotal { get; set; }
        public string SellWebSite { get; set; }
        public string Nots { get; set; }
    }
}
