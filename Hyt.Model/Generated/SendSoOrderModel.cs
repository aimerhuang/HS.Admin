
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2013-09-13 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class SendSoOrderModel
    {
        /// <summary>
        /// 商家发货快递公司编号
        /// </summary>
        [Description("商家发货快递公司编号")]
        public string OverseaCarrier { get; set; }
        /// <summary>
        /// 商家发货快递编号
        /// </summary>
        [Description("商家发货快递编号")]
        public string OverseaTrackingNo { get; set; }
        /// <summary>
        /// 海外仓库编号
        /// </summary>
        [Description("海外仓库编号")]
        public string WarehouseId { get; set; }
        /// <summary>
        /// 客户关联单号
        /// </summary>
        [Description("客户关联单号")]
        public string CustomerReference { get; set; }
        /// <summary>
        /// 来件商家
        /// </summary>
        [Description("来件商家")]
        public string MerchantName { get; set; }
        /// <summary>
        /// 来件商家单号
        /// </summary>
        [Description("来件商家单号")]
        public string MerchantOrderNo { get; set; }
        /// <summary>
        /// 来件包裹面单上的收件人firstname
        /// </summary>
        [Description("来件包裹面单上的收件人firstname")]
        public string ConsigneeFirstName { get; set; }
        /// <summary>
        /// 来件包裹面单上的收件人lasttname
        /// </summary>
        [Description("来件包裹面单上的收件人lasttname")]
        public string ConsigneeLastName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 商品SKU
        /// </summary>
        [Description("商品SKU")]
        public string SKU { get; set; }
        /// <summary>
        /// 国际商品编号
        /// </summary>
        [Description("国际商品编号")]
        public string UPC { get; set; }
        /// <summary>
        /// 货物名称
        /// </summary>
        [Description("货物名称")]
        public string CommodityName { get; set; }
        /// <summary>
        /// 产品分类
        /// </summary>
        [Description("产品分类")]
        public string Category { get; set; }
        /// <summary>
        /// 商品品牌
        /// </summary>
        [Description("商品品牌")]
        public string Brand { get; set; }
        /// <summary>
        /// 商品颜色
        /// </summary>
        [Description("商品颜色")]
        public string Color { get; set; }
        /// <summary>
        /// 商品Size
        /// </summary>
        [Description("商品Size")]
        public string Size { get; set; }
        /// <summary>
        /// 商品材质
        /// </summary>
        [Description("商品材质")]
        public string Material { get; set; }
        /// <summary>
        /// 商品描述详细信息
        /// </summary>
        [Description("商品描述详细信息")]
        public string CommoditySourceURL { get; set; }
        /// <summary>
        /// 商品图片下载路径
        /// </summary>
        [Description("商品图片下载路径")]
        public string CommodityImageUrlList { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        [Description("单价")]
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 申报价值
        /// </summary>
        [Description("申报价值")]
        public decimal DeclaredValue { get; set; }
        /// <summary>
        /// 货币单位
        /// </summary>
        [Description("货币单位")]
        public string ValueUnit { get; set; }
        /// <summary>
        ///商品重量 单重
        /// </summary>
        [Description("商品重量 单重")]
        public decimal Weight { get; set; }
        /// <summary>
        /// 商品重量单位 kg/g/lb/oz
        /// </summary>
        [Description("商品重量单位 kg/g/lb/oz")]
        public string WeightUnit { get; set; }
        /// <summary>
        /// 商品体积规格
        /// </summary>
        [Description("商品体积规格")]
        public string Volume { get; set; }
        /// <summary>
        /// 商品体积单位
        /// </summary>
        [Description("商品体积单位")]
        public string VolumeUnit { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Description("数量")]
        public int Quantity { get; set; }
        /// <summary>
        /// 客户关联单号
        /// </summary>
        [Description("客户关联单号")]
        public string CustomerReferenceSub { get; set; }
    }
}

