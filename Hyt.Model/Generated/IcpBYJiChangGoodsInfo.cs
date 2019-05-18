
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
    public partial class IcpBYJiChangGoodsInfo
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
        /// 企业商品自编号
        /// </summary>
        [Description("企业商品自编号")]
        public string EntGoodsNo { get; set; }
        /// <summary>
        /// 跨境公共平台商品备案申请号
        /// </summary>
        [Description("跨境公共平台商品备案申请号")]
        public string EPortGoodsNo { get; set; }
        /// <summary>
        /// 检验检疫商品备案编号
        /// </summary>
        [Description("检验检疫商品备案编号")]
        public string CIQGoodsNo { get; set; }
        /// <summary>
        /// 海关正式备案编号
        /// </summary>
        [Description("海关正式备案编号")]
        public string CusGoodsNo { get; set; }
        /// <summary>
        /// 上架品名
        /// </summary>
        [Description("上架品名")]
        public string ShelfGName { get; set; }
        /// <summary>
        /// 行邮税号
        /// </summary>
        [Description("行邮税号")]
        public string NcadCode { get; set; }
        /// <summary>
        /// 行邮税名称
        /// </summary>
        [Description("行邮税名称")]
        public string PostTariffName { get; set; }
        /// <summary>
        /// 商品条形码
        /// </summary>
        [Description("商品条形码")]
        public string BarCode { get; set; }
        /// <summary>
        /// HS编码
        /// </summary>
        [Description("HS编码")]
        public string HSCode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Description("商品名称")]
        public string GoodsName { get; set; }
        /// <summary>
        /// 型号规格
        /// </summary>
        [Description("型号规格")]
        public string GoodsStyle { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        [Description("品牌")]
        public string Brand { get; set; }
        /// <summary>
        /// 申报计量单位
        /// </summary>
        [Description("申报计量单位")]
        public string GUnit { get; set; }
        /// <summary>
        /// 第一法定计量单位
        /// </summary>
        [Description("第一法定计量单位")]
        public string StdUnit { get; set; }
        /// <summary>
        /// 第二法定计量单位
        /// </summary>
        [Description("第二法定计量单位")]
        public string SecUnit { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        [Description("单价")]
        public decimal RegPrice { get; set; }
        /// <summary>
        /// 币制
        /// </summary>
        [Description("币制")]
        public string CurrCode { get; set; }
        /// <summary>
        /// 是否赠品
        /// </summary>
        [Description("是否赠品")]
        public int GiftFlag { get; set; }
        /// <summary>
        /// 目的国及原产国
        /// </summary>
        [Description("目的国及原产国")]
        public string OriginCountry { get; set; }
        /// <summary>
        /// 商品品质
        /// </summary>
        [Description("商品品质")]
        public string Quality { get; set; }
        /// <summary>
        /// 品质证明说明
        /// </summary>
        [Description("品质证明说明")]
        public string QualityCertify { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        [Description("生产厂家")]
        public string Manufactory { get; set; }
        /// <summary>
        /// 毛重
        /// </summary>
        [Description("毛重")]
        public decimal NetWt { get; set; }
        /// <summary>
        /// 净重
        /// </summary>
        [Description("净重")]
        public decimal GrossWt { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        [Description("商品描述")]
        public string GNote { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Notes { get; set; }
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

