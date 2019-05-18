using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Inbound
{
    /// <summary>
    /// 进仓货物明细信息
    /// </summary>
    public class InboundContent
    {
        /// <summary>
        /// 流水号
        /// </summary>
        [XmlElement(ElementName = "Seq")]
        public string Seq { get; set; }

        /// <summary>
        /// 商品批次号
        /// </summary>
        [XmlElement(ElementName = "GoodsBatchNo")]
        public string GoodsBatchNo { get; set; }

        /// <summary>
        /// 企业商品货号
        /// </summary>
        [XmlElement(ElementName = "EntGoodsNo")]
        public string EntGoodsNo { get; set; }

        /// <summary>
        /// 检验检疫商品备案编号
        /// </summary>
        [XmlElement(ElementName = "CIQGoodsNo")]
        public string CIQGoodsNo { get; set; }

        /// <summary>
        /// 海关正式备案编号
        /// </summary>
        [XmlElement(ElementName = "CusGoodsNo")]
        public string CusGoodsNo { get; set; }

        /// <summary>
        /// HS编码
        /// </summary>
        [XmlElement(ElementName = "HSCode")]
        public string HSCode { get; set; }

        /// <summary>
        /// 原产国/地区
        /// </summary>
        [XmlElement(ElementName = "OriginCountry")]
        public string OriginCountry { get; set; }

        /// <summary>
        /// 采购方式地点
        /// </summary>
        [XmlElement(ElementName = "PurchasePlace")]
        public string PurchasePlace { get; set; }

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
        /// 标准计量单位
        /// </summary>
        [XmlElement(ElementName = "StdUnit")]
        public string StdUnit { get; set; }

        /// <summary>
        /// 生产厂家
        /// </summary>
        [XmlElement(ElementName = "Manufactory")]
        public string Manufactory { get; set; }

        /// <summary>
        /// 标准数量
        /// </summary>
        [XmlElement(ElementName = "StdQty")]
        public string StdQty { get; set; }

        /// <summary>
        /// 申报单价
        /// </summary>
        [XmlElement(ElementName = "DeclarePrice")]
        public string DeclarePrice { get; set; }

        /// <summary>
        /// 申报总价
        /// </summary>
        [XmlElement(ElementName = "DecTotal")]
        public string DecTotal { get; set; }

        /// <summary>
        /// 贸易国别
        /// </summary>
        [XmlElement(ElementName = "TradeCountry")]
        public string TradeCountry { get; set; }

        /// <summary>
        /// 包装种类(外包装)
        /// </summary>
        [XmlElement(ElementName = "PackageType")]
        public string PackageType { get; set; }

        /// <summary>
        /// 毛重
        /// </summary>
        [XmlElement(ElementName = "GrossWeight")]
        public string GrossWeight { get; set; }

        /// <summary>
        /// 净重
        /// </summary>
        [XmlElement(ElementName = "NetWeight")]
        public string NetWeight { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [XmlElement(ElementName = "Notes")]
        public string Notes { get; set; }

    }
}
