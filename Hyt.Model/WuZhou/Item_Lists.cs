using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Hyt.Model.WuZhou
{
    public class Item_Lists
    {
        /// <summary>
        /// Sku编码
        /// </summary>
        [Description("Sku编码")]
        public string sku_code { get; set; }

        /// <summary>
        /// 海关商品编号
        /// </summary>
        [Description("海关商品编号")]
        public string chcus_sku { get; set; }

        /// <summary>
        /// Sku单价
        /// </summary>
        [Description("Sku单价")]
        public decimal sku_price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Description("数量")]
        public int qty { get; set; }

        /// <summary>
        /// 单品金额小计
        /// </summary>
        [Description("单品金额小计")]
        public decimal total { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        [Description("折扣")]
        public double discount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string note { get; set; }
    }
}
