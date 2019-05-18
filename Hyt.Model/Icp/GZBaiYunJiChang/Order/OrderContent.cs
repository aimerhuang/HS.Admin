using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Order
{
    /// <summary>
    /// 一份订单内容
    /// </summary>
    public class OrderContent
    {
        [XmlElement(ElementName = "OrderDetail")]
        public OrderDetail OrderDetail { get; set; }

        [XmlElement(ElementName = "OrderWaybillRel")]
        public OrderWaybillRel OrderWaybillRel { get; set; }

        [XmlElement(ElementName = "OrderPaymentRel")]
        public OrderPaymentRel OrderPaymentRel { get; set; }
    }
}
