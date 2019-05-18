using System;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 商品条码
    /// </summary>
    public class PdProductBarcode
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 条码前缀
        /// </summary>
        [Description("条码前缀")]
        public string Prefix { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        [Description("商品条码")]
        public string Barcode { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        [Description("商品编号")]
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Description("数量")]
        public int ProductQuantity { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        [Description("更新人")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Description("更新时间")]
        public DateTime LastUpdateDate { get; set; }
    }
}
