using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods
{
    public class GoodsRegHead
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
        /// 电商企业编号
        /// </summary>
        [XmlElement(ElementName = "EBEntNo")]
        public string EBEntNo { get; set; }

        /// <summary>
        /// 电商企业名称
        /// </summary>
        [XmlElement(ElementName = "EBEntName")]
        public string EBEntName { get; set; }

        /// <summary>
        /// 操作方式
        /// </summary>
        [XmlElement(ElementName = "OpType")]
        public string OpType { get; set; }

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

        /// <summary>
        /// 电商平台企业编号可空
        /// </summary>
        [XmlElement(ElementName = "EBPEntNo")]
        public string EBPEntNo { get; set; }

        /// <summary>
        /// 电商平台名称 可空
        /// </summary>
        [XmlElement(ElementName = "EBPEntName")]
        public string EBPEntName { get; set; }

        /// <summary>
        /// 物流企业编号 可空
        /// </summary>
        [XmlElement(ElementName = "EHSEntNo")]
        public string EHSEntNo { get; set; }

        /// <summary>
        /// 物流企业名称 可空
        /// </summary>
        [XmlElement(ElementName = "EHSEntName")]
        public string EHSEntName { get; set; }

        /// <summary>
        /// 币制代码
        /// </summary>
        [XmlElement(ElementName = "CurrCode")]
        public string CurrCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(ElementName = "BusinessType")]
        public string BusinessType { get; set; }

        /// <summary>
        /// 录入日期
        /// </summary>
        [XmlElement(ElementName = "InputDate")]
        public string InputDate { get; set; }

        /// <summary>
        /// 申请备案时间
        /// </summary>
        [XmlElement(ElementName = "DeclDate")]
        public string DeclDate { get; set; }

        /// <summary>
        /// 进出境标志
        /// </summary>
        [XmlElement(ElementName = "IeFlag")]
        public string IeFlag { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [XmlElement(ElementName = "Notes")]
        public string Notes { get; set; }
    }
}
