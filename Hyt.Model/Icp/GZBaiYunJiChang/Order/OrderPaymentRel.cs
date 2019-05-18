using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Order
{
    /// <summary>
    /// 电子订单关联支付单
    /// </summary>
    /// 
    public class OrderPaymentRel
    {
        /// <summary>
        /// 支付企业代码
        /// </summary>
        /// 
        [XmlElement(ElementName = "PayEntNo")]
        public string PayEntNo { get; set; }

        /// <summary>
        /// 支付企业名称
        /// </summary>
        /// 
        [XmlElement(ElementName = "PayEntName")]
        public string PayEntName { get; set; }

        /// <summary>
        /// 支付交易类型
        /// </summary>
        /// 
        [XmlElement(ElementName = "PayType")]
        public string PayType { get; set; }

        /// <summary>
        /// 支付交易编号
        /// </summary>
        /// 
        [XmlElement(ElementName = "PayNo")]
        public string PayNo { get; set; }

        /// <summary>
        /// 备注 可空
        /// </summary>
        /// 
        [XmlElement(ElementName = "Notes")]
        public string Notes { get; set; }
    }
}
