using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.InventorySheet
{
    /// <summary>
    /// 商品规格报价
    /// </summary>
    public class PdProductSpecPrices
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
        public string SpecValue { get; set; }

        /// 仓库系统编号
        /// </summary>
        [Description("仓库系统编号")]
        public int? WarehouseSysNo { get; set; }


        /// 规格单位
        /// </summary>
        [Description("规格单位")]
        public string Unit { get; set; }


        /// 规格毛重(Kg）
        /// </summary>
        [Description("规格毛重(Kg）")]
        public decimal? GrossWeight { get; set; }

        /// 规格体积(m3)
        /// </summary>
        [Description("规格体积(m3)")]
        public decimal? Volume { get; set; }


        /// 规格单价
        /// </summary>
        [Description("规格单价")]
        public decimal? Price { get; set; }


        /// 规格总价
        /// </summary>
        [Description("规格总价")]
        public decimal? TotalPrice { get; set; }


        /// 打包费
        /// </summary>
        [Description("打包费")]
        public decimal? PackingFee { get; set; }


        /// 折合总价
        /// </summary>
        [Description("折合总价")]
        public decimal? EquivalentPrice { get; set; }

        /// 折合单价
        /// </summary>
        [Description("折合单价")]
        public decimal? EquivalentTotalPrice { get; set; }

        /// 销售价
        /// </summary>
        [Description("销售价")]
        public decimal? SalesPrice { get; set; }

        #region 扩展属性
        /// <summary>
        /// 规格值集合
        /// </summary>
        public PdProductSpecValues SpecValueList { get; set; }
        #endregion
    }
}
