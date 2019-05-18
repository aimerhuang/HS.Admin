using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha
{
   /// <summary>
    /// 商品信息
    /// </summary>
    public class Record1
    {
        /// <summary>
        /// 商品货号
        /// </summary>
        [XmlElement(ElementName = "Gcode")]
        public string Gcode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [XmlElement(ElementName = "Gname")]
        public string Gname { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [XmlElement(ElementName = "Spec")]
        public string Spec { get; set; }
        /// <summary>
        /// 商品HS编码
        /// </summary>
        [XmlElement(ElementName = "Hscode")]
        public string Hscode { get; set; }
        /// <summary>
        /// 计量单位(最小)
        /// </summary>
        [XmlElement(ElementName = "Unit")]
        public string Unit { get; set; }
        /// <summary>
        /// 商品条形码
        /// </summary>
        [XmlElement(ElementName = "GoodsBarcode")]
        public string GoodsBarcode { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        [XmlElement(ElementName = "GoodsDesc")]
        public string GoodsDesc { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [XmlElement(ElementName = "Remark")]
        public string Remark { get; set; }
        /// <summary>
        /// 生产企业名称
        /// </summary>
        [XmlElement(ElementName = "ComName")]
        public string ComName { get; set; }
        /// <summary>
        /// 原产国/地区
        /// </summary>
        [XmlElement(ElementName = "Brand")]
        public string Brand { get; set; }
        /// <summary>
        /// 成分  为空时默认“无”
        /// </summary>
        [XmlElement(ElementName = "AssemCountry")]
        public string AssemCountry { get; set; }
        /// <summary>
        /// 成分  为空时默认“无”
        /// </summary>
        [XmlElement(ElementName = "Ingredient")]
        public string Ingredient { get; set; }
        /// <summary>
        /// 超范围使用食品添加剂为空时默认“无”
        /// </summary>
        [XmlElement(ElementName = "Additiveflag")]
        public string Additiveflag { get; set; }
        /// <summary>
        /// 含有毒害物质为空时默认“无”
        /// </summary>
        [XmlElement(ElementName = "Poisonflag")]
        public string Poisonflag { get; set; }
    }
}
