using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiSupply.HYH
{
    public class ProductResponse
    {
        public string SkuId { get; set; }
        public string Name { get; set; }
        public string SkuName { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public decimal MarketPrice { get; set; }

        public int Weight { get; set; }
        public string DefaultImage { get; set; }
        public List<string> ImageList { get; set; }

        public string Remark { get; set; }
        public int TradeMode { get; set; }
        public int StockQuantity { get; set; }
        public DateTime UpdateTime { get; set; }
        public string WarehouseName { get; set; }
    }
}
