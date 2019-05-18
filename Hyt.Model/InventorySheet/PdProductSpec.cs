using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.InventorySheet
{
    /// <summary>
    /// 商品供货规格
    /// </summary>
    public class PdProductSpec
    {
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int? SysNo { get; set; }

        /// 产品系统编号
        /// </summary>
        [Description("产品系统编号")]
        public int? ProductSysNo { get; set; }

        /// 规格值
        /// </summary>
        [Description("规格值")]
        public string SpecValues { get; set; }
    }

    /// <summary>
    /// 规格值
    /// </summary>
    public class PdProductSpecValues 
    {
        /// <summary>
        /// 包装类型
        /// </summary>
        public string unit { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string spec { get; set; }

        /// <summary>
        /// price
        /// </summary>
        public decimal? price { get; set; }
    }
}
