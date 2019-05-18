using Hyt.Model.Icp.GZNanSha.CARGO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha
{
    /// <summary>
    /// 企业商品备案主表信息
    /// </summary>
    public class Record
    {
        /// <summary>
        /// 商品申请编号
        /// </summary>
        [XmlElement(ElementName = "CargoBcode")]
        public string CargoBcode { get; set; }
        /// <summary>
        /// 国检组织机构代码
        /// </summary>
        [XmlElement(ElementName = "Ciqbcode")]
        public string Ciqbcode { get; set; }
        /// <summary>
        /// 跨境电商企业备案号
        /// </summary>
        [XmlElement(ElementName = "CbeComcode")]
        public string CbeComcode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [XmlElement(ElementName = "Remark")]
        public string Remark { get; set; }
        /// <summary>
        /// 制单企业的企业备案号
        /// </summary>
        [XmlElement(ElementName = "Editccode")]
        public string Editccode { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        [XmlElement(ElementName = "OperType")]
        public string OperType { get; set; }

        /// <summary>
        /// 企业商品备案子表信息
        /// </summary>
        [XmlElement(ElementName = "CARGOLIST")]
        public CARGOLIST cargoList { get; set; }
    }
}
