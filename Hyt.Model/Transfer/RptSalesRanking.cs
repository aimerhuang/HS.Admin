
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 报表-销售排行
    /// </summary>
    /// <remarks>2013-10-22 朱家宏创建</remarks>
    public class RptSalesRanking
    {
        /// <summary>
        /// 序号
        /// </summary>
        [Description("序号")]
        public int RowNumber { get; set; }
        /// <summary>
        /// 商品类别名称
        /// </summary>
        [Description("经销商名称")]
        public string DealerName { get; set; }
        /// <summary>
        /// 商品类别名称
        /// </summary>
        [Description("仓库名称")]
        public string WarehouseName { get; set; }
        // <summary>
        /// 商品类别名称
        /// </summary>
        [Description("类型")]
        public string SaleProType { get; set; }
        /// <summary>
        /// 商品类别名称
        /// </summary>
        [Description("商品类别名称")]
        public string ProductCategoryName { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        [Description("商品编码")]
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Description("商品条码")]
        public string Barcode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Description("商品名称")]
        public string ProductName { get; set; }
        /// <summary>
        /// 销售量
        /// </summary>
        [Description("销售量")]
        public int SalesQuantity { get; set; }
        /// <summary>
        /// 销售额
        /// </summary>
        [Description("销售额")]
        public decimal SalesAmount { get; set; }
        /// <summary>
        /// 商品分类编码
        /// </summary>
        [Description("商品分类编码")]
        public string ProductCategorySysNos { get; set; }

        public float ReturnNum { get; set; }
        public float ReturnTotalValue { get; set; }

        public decimal StockQuantity { get; set; }

    }
}
