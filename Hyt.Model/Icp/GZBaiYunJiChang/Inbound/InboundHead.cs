using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Inbound
{
    public class InboundHead
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
        /// 企业入仓申报编号
        /// </summary>
        [XmlElement(ElementName = "EntInboundNo")]
        public string EntInboundNo { get; set; }

        /// <summary>
        /// 公共平台入仓编号
        /// </summary>
        [XmlElement(ElementName = "EportInboundNo")]
        public string EportInboundNo { get; set; }

        /// <summary>
        /// 检验检疫入仓编号
        /// </summary>
        [XmlElement(ElementName = "CIQInboundNo")]
        public string CIQInboundNo { get; set; }

        /// <summary>
        /// 海关入仓编号
        /// </summary>
        [XmlElement(ElementName = "CusInboundNo")]
        public string CusInboundNo { get; set; }

        /// <summary>
        /// 货物存放地
        /// </summary>
        [XmlElement(ElementName = "DischargePlace")]
        public string DischargePlace { get; set; }

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
        /// 对应单据类型
        /// </summary>
        [XmlElement(ElementName = "CorrtDocType")]
        public string CorrtDocType { get; set; }

        /// <summary>
        /// 对应单证编号
        /// </summary>
        [XmlElement(ElementName = "CorrDocCode")]
        public string CorrDocCode { get; set; }

        /// <summary>
        /// 账册编号
        /// </summary>
        [XmlElement(ElementName = "EmsNo")]
        public string EmsNo { get; set; }

        /// <summary>
        /// 监管方式
        /// </summary>
        [XmlElement(ElementName = "TradeMode")]
        public string TradeMode { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        [XmlElement(ElementName = "Recipient")]
        public string Recipient { get; set; }

        /// <summary>
        /// 发货人
        /// </summary>
        [XmlElement(ElementName = "Shipper")]
        public string Shipper { get; set; }

        /// <summary>
        /// 进境口岸
        /// </summary>
        [XmlElement(ElementName = "InboundPort")]
        public string InboundPort { get; set; }

        /// <summary>
        /// 起抵国
        /// </summary>
        [XmlElement(ElementName = "CountryCode")]
        public string CountryCode { get; set; }

        /// <summary>
        /// 进出仓日期
        /// </summary>
        [XmlElement(ElementName = "InboundDate")]
        public string InboundDate { get; set; }

        /// <summary>
        /// 录入日期
        /// </summary>
        [XmlElement(ElementName = "InputDate")]
        public string InputDate { get; set; }

        /// <summary>
        /// 操作方式
        /// </summary>
        [XmlElement(ElementName = "OpType")]
        public string OpType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [XmlElement(ElementName = "Notes")]
        public string Notes { get; set; }
    }
}
