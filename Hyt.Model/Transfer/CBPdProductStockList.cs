using Hyt.Model.InventorySheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    public class CBPdProductStockList : PdProductStockList
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        public decimal Price { get; set; }

        public string DetailStockQuantity { get; set; }
        /// <summary>
        /// 分类集合
        /// </summary>
        public string PdCategorySysNos { get; set; }
        // <summary>
        /// 待发货
        /// </summary>
        public int ProductQuantity { get; set; }
        /// <summary>
        /// "Kis库存
        /// </summary>
        public string KisStock { get; set; }


        /// <summary>
        /// 商品规格报价 2017/9/6 吴琨 添加 
        /// 用于显示商品规格报价
        /// </summary>
        public List<PdProductSpecPrices> SpecPricesList { get; set; }
    }
}
