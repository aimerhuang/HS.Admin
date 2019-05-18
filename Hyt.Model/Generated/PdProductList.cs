
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2013-11-04 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class PdProductList
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int BrandSysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        [Description("商品编号")]
        public string ErpCode { get; set; }
        /// <summary>
        /// 后台显示名称
        /// </summary>
        [Description("后台显示名称")]
        public string EasName { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        [Description("条码")]
        public string Barcode { get; set; }
        /// <summary>
        /// 二维码
        /// </summary>
        [Description("二维码")]
        public string QRCode { get; set; }
        /// <summary>
        /// 商品类型：普通商品（10）、虚拟商品（20）
        /// </summary>
        [Description("商品类型：普通商品（10）、虚拟商品（20）")]
        public int ProductType { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Description("商品名称")]
        public string ProductName { get; set; }
        /// <summary>
        /// 商品副名称
        /// </summary>
        [Description("商品副名称")]
        public string ProductSubName { get; set; }
        /// <summary>
        /// 商品名称拼音
        /// </summary>
        [Description("商品名称拼音")]
        public string NameAcronymy { get; set; }
        /// <summary>
        /// 商品简称
        /// </summary>
        [Description("商品简称")]
        public string ProductShortTitle { get; set; }
        /// <summary>
        /// 商品简介
        /// </summary>
        [Description("商品简介")]
        public string ProductSummary { get; set; }
        /// <summary>
        /// 商品广告语
        /// </summary>
        [Description("商品广告语")]
        public string ProductSlogan { get; set; }
        /// <summary>
        /// 包装清单
        /// </summary>
        [Description("包装清单")]
        public string PackageDesc { get; set; }
        /// <summary>
        /// 产品描述
        /// </summary>
        [Description("产品描述")]
        public string ProductDesc { get; set; }
        /// <summary>
        /// 商品图片地址
        /// </summary>
        [Description("商品图片地址")]
        public string ProductImage { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        [Description("浏览次数")]
        public int ViewCount { get; set; }
        /// <summary>
        /// SeoTitle
        /// </summary>
        [Description("SeoTitle")]
        public string SeoTitle { get; set; }
        /// <summary>
        /// SeoKeyword
        /// </summary>
        [Description("SeoKeyword")]
        public string SeoKeyword { get; set; }
        /// <summary>
        /// SeoDescription
        /// </summary>
        [Description("SeoDescription")]
        public string SeoDescription { get; set; }
        /// <summary>
        /// 状态：上架（1）、下架（0）
        /// </summary>
        [Description("状态：上架（1）、下架（0）")]
        public int Status { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 是否允许前台下单：是（1）、否（0）
        /// </summary>
        [Description("是否允许前台下单：是（1）、否（0）")]
        public int CanFrontEndOrder { get; set; }
        /// <summary>
        /// 是否前台显示：是（1）、否（0）
        /// </summary>
        [Description("是否前台显示：是（1）、否（0）")]
        public int IsFrontDisplay { get; set; }
        /// <summary>
        /// 代理商
        /// </summary>
        [Description("代理商")]
        public int AgentSysNo { get; set; }
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
        /// <summary>
        /// 时间戳
        /// </summary>
        [Description("时间戳")]
        public DateTime Stamp { get; set; }
        /// <summary>
        /// 国家编号
        /// </summary>
        [Description("国家编号")]
        public int OriginSysNo { get; set; }
        /// <summary>
        /// 毛重
        /// </summary>
        [Description("毛重")]
        public decimal GrosWeight { get; set; }
        /// <summary>
        /// 净重
        /// </summary>
        [Description("净重")]
        public decimal NetWeight { get; set; }
        /// <summary>
        /// 销售计量单位
        /// </summary>
        [Description("销售计量单位")]
        public string SalesMeasurementUnit { get; set; }
        /// <summary>
        /// 申报要素
        /// </summary>
        [Description("申报要素")]
        public string ProductDeclare { get; set; }
        /// <summary>
        /// 销售网址
        /// </summary>
        [Description("销售网址")]
        public string SalesAddress { get; set; }
        /// <summary>
        /// 货币单位
        /// </summary>
        [Description("货币单位")]
        public string ValueUnit { get; set; }
        /// <summary>
        /// 体积规格
        /// </summary>
        [Description("体积规格")]
        public string Volume { get; set; }
        /// <summary>
        /// 体积单位
        /// </summary>
        [Description("体积单位")]
        public string VolumeUnit { get; set; }
        /// <summary>
        /// 税费
        /// </summary>
        [Description("税费")]
        public string Tax { get; set; }
        /// <summary>
        /// 是否选用运费模板
        /// </summary>
        [Description("是否选用运费模板")]
        public string FreightFlag { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        [Description("运费")]
        public decimal Freight { get; set; }
        /// <summary>
        /// 基础价格类
        /// </summary>
        [Description("基础价格类")]
        public PdPrice PdPrice { get; set; }
        /// <summary>
        /// 会员价格类
        /// </summary>
        [Description("会员价格类")]
        public PdPrice PdSalePrice { get; set; }

        /// <summary>
        /// VIP会员价
        /// </summary>
        [Description("VIP会员价")]
        public PdPrice PdVIPPrice { get; set; }

        /// <summary>
        /// 钻石会员价
        /// </summary>
        [Description("钻石会员价")]
        public PdPrice PdDiamondPrice { get; set; }
        
        /// <summary>
        /// 钻石会员价
        /// </summary>
        [Description("销售合伙人")]
        public PdPrice SaleUserPrice { get; set; }

        /// <summary>
        /// 门店销售价类
        /// </summary>
        [Description("门店销售价类")]
        public PdPrice PdStoreSalePrice { get; set; }
        /// <summary>
        /// 成本价
        /// </summary>
        [Description("成本价")]
        public decimal CostPrice { get; set; }
        /// <summary>
        /// 类目类
        /// </summary>
        [Description("类目类")]
        public PdCategorySql PdCategorySql { get; set; }

        /// <summary>
        /// 商品类目类
        /// </summary>
        [Description("商品类目类")]
        public PdCategoryAssociation PdCategoryAssociation { get; set; }
        /// <summary>
        /// 利润比例
        /// </summary>
        [Description("利润比例")]
        public decimal PriceRate { get; set; }
        /// <summary>
        /// 利润比例
        /// </summary>
        [Description("利润比例")]
        public decimal PriceValue { get; set; }
        /// <summary>
        /// 批发价
        /// </summary>
        [Description("批发价")]
        public decimal TradePrice { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        [Description("经销商编号")]
        public int DealerSysNo { get; set; }

        /// <summary>
        /// 经销商编号
        /// </summary>
        [Description("虚拟销售量")]
        public int Sales { get; set; }
    }
}

