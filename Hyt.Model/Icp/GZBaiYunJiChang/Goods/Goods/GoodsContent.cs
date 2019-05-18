using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public class GoodsContent
    {
        /// <summary>
        /// 商品序号
        /// </summary>
        [XmlElement(ElementName = "Seq")]
        public string Seq { get; set; }

        /// <summary>
        /// 企业商品自编号
        /// </summary>
        [XmlElement(ElementName = "EntGoodsNo")]
        public string EntGoodsNo { get; set; }

        /// <summary>
        /// 跨境公共平台商品备案申请号 可空
        /// </summary>
        [XmlElement(ElementName = "EPortGoodsNo")]
        public string EPortGoodsNo { get; set; }

        /// <summary>
        /// 检验检疫商品备案编号 可空
        /// </summary>
        [XmlElement(ElementName = "CIQGoodsNo")]
        public string CIQGoodsNo { get; set; }

        /// <summary>
        /// 海关正式备案编号  可空
        /// </summary>
        [XmlElement(ElementName = "CusGoodsNo")]
        public string CusGoodsNo { get; set; }

        /// <summary>
        /// 账册号  可空 后续版本必填
        /// </summary>
        [XmlElement(ElementName = "EmsNo")]
        public string EmsNo { get; set; }

        /// <summary>
        /// 保税账册里的项号  可空 后续版本必填
        /// </summary>
        [XmlElement(ElementName = "ItemNo")]
        public string ItemNo { get; set; }

        /// <summary>
        /// 上架品名
        /// </summary>
        [XmlElement(ElementName = "ShelfGName")]
        public string ShelfGName { get; set; }

        /// <summary>
        /// 行邮税号
        /// </summary>
        [XmlElement(ElementName = "NcadCode")]
        public string NcadCode { get; set; }

        /// <summary>
        /// 行邮税名称
        /// </summary>
        [XmlElement(ElementName = "PostTariffName")]
        public string PostTariffName { get; set; }

        /// <summary>
        /// 商品条形码 可空
        /// </summary>
        [XmlElement(ElementName = "BarCode")]
        public string BarCode { get; set; }

        /// <summary>
        /// HS编码
        /// </summary>
        [XmlElement(ElementName = "HSCode")]
        public string HSCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [XmlElement(ElementName = "GoodsName")]
        public string GoodsName { get; set; }

        /// <summary>
        /// 型号规格
        /// </summary>
        [XmlElement(ElementName = "GoodsStyle")]
        public string GoodsStyle { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        [XmlElement(ElementName = "Brand")]
        public string Brand { get; set; }

        /// <summary>
        /// 申报计量单位
        /// </summary>
        [XmlElement(ElementName = "GUnit")]
        public string GUnit { get; set; }

        /// <summary>
        /// 第一法定计量单位
        /// </summary>
        [XmlElement(ElementName = "StdUnit")]
        public string StdUnit { get; set; }

        /// <summary>
        /// 第二法定计量单位 可空
        /// </summary>
        [XmlElement(ElementName = "SecUnit")]
        public string SecUnit { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [XmlElement(ElementName = "RegPrice")]
        public string RegPrice { get; set; }

        /// <summary>
        /// 币制
        /// </summary>
        [XmlElement(ElementName = "CurrCode")]
        public string CurrCode { get; set; }

        /// <summary>
        /// 是否赠品
        /// </summary>
        [XmlElement(ElementName = "GiftFlag")]
        public string GiftFlag { get; set; }

        /// <summary>
        /// 目的国及原产国
        /// </summary>
        [XmlElement(ElementName = "OriginCountry")]
        public string OriginCountry { get; set; }

        /// <summary>
        /// 商品品质
        /// </summary>
        [XmlElement(ElementName = "Quality")]
        public string Quality { get; set; }

        /// <summary>
        /// 品质证明说明 可空
        /// </summary>
        [XmlElement(ElementName = "QualityCertify")]
        public string QualityCertify { get; set; }

        /// <summary>
        /// 生产厂家
        /// </summary>
        [XmlElement(ElementName = "Manufactory")]
        public string Manufactory { get; set; }

        /// <summary>
        /// 净重
        /// </summary>
        [XmlElement(ElementName = "NetWt")]
        public string NetWt { get; set; }

        /// <summary>
        /// 毛重
        /// </summary>
        [XmlElement(ElementName = "GrossWt")]
        public string GrossWt { get; set; }

        /// <summary>
        /// 商品描述 可空
        /// </summary>
        [XmlElement(ElementName = "GNote")]
        public string GNote { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        [XmlElement(ElementName = "ValidDate")]
        public string ValidDate { get; set; }

        /// <summary>
        /// 失效日期
        /// </summary>
        [XmlElement(ElementName = "EndDate")]
        public string EndDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [XmlElement(ElementName = "Notes")]
        public string Notes { get; set; }
    }
}
