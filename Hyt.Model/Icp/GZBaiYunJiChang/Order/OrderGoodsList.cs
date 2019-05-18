using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Order
{
    public class OrderGoodsList
    {
        /// <summary>
        /// 商品序号
        /// </summary>
        [XmlElement(ElementName = "Seq")]
        public string Seq { get; set; }

        /// <summary>
        /// 物流订单编号
        /// </summary>
        [XmlElement(ElementName = "LogisticsOrderNo")]
        public string LogisticsOrderNo { get; set; }

        /// <summary>
        /// 企业商品货号
        /// </summary>
        [XmlElement(ElementName = "EntGoodsNo")]
        public string EntGoodsNo { get; set; }

        /// <summary>
        /// 跨境公共平台商品备案申请号  可空
        /// </summary>
        [XmlElement(ElementName = "EPortGoodsNo")]
        public string EPortGoodsNo { get; set; }

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
        /// 商品名称
        /// </summary>
        [XmlElement(ElementName = "GoodsName")]
        public string GoodsName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        [XmlElement(ElementName = "GoodsStyle")]
        public string GoodsStyle { get; set; }

        /// <summary>
        /// 商品条形码 可空
        /// </summary>
        [XmlElement(ElementName = "BarCode")]
        public string BarCode { get; set; }

        /// <summary>
        /// 品牌  可空
        /// </summary>
        [XmlElement(ElementName = "Brand")]
        public string Brand { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [XmlElement(ElementName = "Qty")]
        public string Qty { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        [XmlElement(ElementName = "Unit")]
        public string Unit { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [XmlElement(ElementName = "Price")]
        public string Price { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        [XmlElement(ElementName = "Total")]
        public string Total { get; set; }

        /// <summary>
        /// 币制
        /// </summary>
        [XmlElement(ElementName = "CurrCode")]
        public string CurrCode { get; set; }

        /// <summary>
        /// 备注 可空
        /// </summary>
        [XmlElement(ElementName = "Notes")]
        public string Notes { get; set; }
    }
}
