
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
    public partial class IcpGZNanShaGoodsInfo
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
        /// 检验检疫商品备案编号
        /// </summary>
        [Description("检验检疫商品备案编号")]
        public string CIQGoodsNo { get; set; }
        /// <summary>
        /// 商品货号
        /// </summary>
        [Description("商品货号")]
        public string Gcode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Description("商品名称")]
        public string Gname { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [Description("规格型号")]
        public string Spec { get; set; }
        /// <summary>
        /// HS编码
        /// </summary>
        [Description("HS编码")]
        public string HSCode { get; set; }
        /// <summary>
        /// 计量单位(最小)
        /// </summary>
        [Description("计量单位(最小)")]
        public string Unit { get; set; }
        /// <summary>
        /// 商品条形码
        /// </summary>
        [Description("商品条形码")]
        public string GoodsBarcode { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        [Description("商品描述")]
        public string GoodsDesc { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 生产企业名称
        /// </summary>
        [Description("生产企业名称")]
        public string ComName { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        [Description("品牌")]
        public string Brand { get; set; }
        /// <summary>
        /// 原产国/地区
        /// </summary>
        [Description("原产国/地区")]
        public string AssemCountry { get; set; }
        /// <summary>
        /// 成分
        /// </summary>
        [Description("成分")]
        public string Ingredient { get; set; }
        /// <summary>
        /// 超范围使用食品添加剂
        /// </summary>
        [Description("超范围使用食品添加剂")]
        public string Additiveflag { get; set; }
        /// <summary>
        /// 含有毒害物质
        /// </summary>
        [Description("含有毒害物质")]
        public string Poisonflag { get; set; }
        /// <summary>
        /// 销售网址
        /// </summary>
        [Description("销售网址")]
        public string SellWebSite { get; set; }
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

