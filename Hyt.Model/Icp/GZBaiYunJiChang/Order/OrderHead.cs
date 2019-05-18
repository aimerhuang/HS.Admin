using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Order
{
    public class OrderHead
    {
        /// <summary>
        /// 申报企业编号
        /// </summary>
        [XmlElement(ElementName = "DeclEntNo")]
        public string DeclEntNo { get; set; }

        /// <summary>
        /// 申报企业名称
        /// </summary>
        [XmlElement(ElementName = "DeclEntName")]
        public string DeclEntName { get; set; }

        /// <summary>
        /// 报送者名称（个人）
        /// </summary>
        [XmlElement(ElementName = "DeclPerson")]
        public string DeclPerson { get; set; }

        /// <summary>
        /// 报送者证件号码（个人）
        /// </summary>
        [XmlElement(ElementName = "DeclPerNumber")]
        public string DeclPerNumber { get; set; }

        /// <summary>
        /// 报送者证件类型代码（个人）
        /// </summary>
        [XmlElement(ElementName = "DeclPerTypeCode")]
        public string DeclPerTypeCode { get; set; }

        /// <summary>
        /// 电商企业编号
        /// </summary>
        [XmlElement(ElementName = "EBEntNo")]
        public string EBEntNo { get; set; }

        /// <summary>
        /// 电商企业名称  可空
        /// </summary>
        [XmlElement(ElementName = "EBEntName")]
        public string EBEntName { get; set; }

        /// <summary>
        /// 电商平台企业编号
        /// </summary>
        [XmlElement(ElementName = "EBPEntNo")]
        public string EBPEntNo { get; set; }

        /// <summary>
        /// 电商平台互联网域名
        /// </summary>
        [XmlElement(ElementName = "InternetDomainName")]
        public string InternetDomainName { get; set; }

        /// <summary>
        /// 电商平台企业名称
        /// </summary>
        [XmlElement(ElementName = "EBPEntName")]
        public string EBPEntName { get; set; }

        /// <summary>
        /// 申报时间
        /// </summary>
        [XmlElement(ElementName = "DeclTime")]
        public string DeclTime { get; set; }

        /// <summary>
        /// 操作方式
        /// </summary>
        [XmlElement(ElementName = "OpType")]
        public string OpType { get; set; }

        /// <summary>
        /// 进出口标示
        /// </summary>
        [XmlElement(ElementName = "IeFlag")]
        public string IeFlag { get; set; }

        /// <summary>
        /// 主管海关代码
        /// </summary>
        [XmlElement(ElementName = "CustomsCode")]
        public string CustomsCode { get; set; }

        /// <summary>
        /// 检验检疫机构代码
        /// </summary>
        [XmlElement(ElementName = "CIQOrgCode")]
        public string CIQOrgCode { get; set; }
    }
}
