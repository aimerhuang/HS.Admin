
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2016-4-02 王耀发 T4生成
    /// </remarks>
    [Serializable]
    public partial class LgGaoJieGoodsInfo
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        [Description("商品编号")]
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        [Description("税号")]
        public string goods_ptcode { get; set; }
        /// <summary>
        /// 物品名称
        /// </summary>
        [Description("物品名称")]
        public string goods_name { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        [Description("品牌")]
        public string brand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [Description("规格型号")]
        public string goods_spec { get; set; }
        /// <summary>
        /// 原产国代码
        /// </summary>
        [Description("原产国代码")]
        public string ycg_code { get; set; }
        /// <summary>
        /// HS编码
        /// </summary>
        [Description("HS编码")]
        public string hs_code { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        [Description("条形码")]
        public string goods_barcode { get; set; }
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
        /// 最后更新人
        /// </summary>
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }
    }
}

