using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    public class CBOutputPdProductStocksYS
    {

        public string 仓库名称 { get; set; }
        public string 商品编码 { get; set; }
        public string 后台显示名称 { get; set; }
        public string 条形码 { get; set; }
        public string 商品SKU { get; set; }
        public string 海关备案号 { get; set; }
        public decimal 采购价格 { get; set; }
        public decimal 库存数量 { get; set; }
    }
}
