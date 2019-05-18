using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha.Order
{
    public class CustomOrderMod
    {
        [XmlElement(ElementName = "EntInsideNo")]
        public string EntInsideNo { get; set; }

        [XmlElement(ElementName = "Ciqbcode")]
        public string Ciqbcode { get; set; }

        [XmlElement(ElementName = "CbeComcode")]
        public string CbeComcode { get; set; }

        [XmlElement(ElementName = "CbepComcode")]
        public string CbepComcode { get; set; }

        [XmlElement(ElementName = "OrderStatus")]
        public string OrderStatus { get; set; }

        [XmlElement(ElementName = "ReceiveName")]
        public string ReceiveName { get; set; }

        [XmlElement(ElementName = "ReceiveAddr")]
        public string ReceiveAddr { get; set; }

        [XmlElement(ElementName = "ReceiveNo")]
        public string ReceiveNo { get; set; }

        [XmlElement(ElementName = "ReceivePhone")]
        public string ReceivePhone { get; set; }

        [XmlElement(ElementName = "FCY")]
        public decimal FCY { get; set; }

        [XmlElement(ElementName = "Fcode")]
        public string Fcode { get; set; }

        [XmlElement(ElementName = "Editccode")]
        public string Editccode { get; set; }

        [XmlElement(ElementName = "DrDate")]
        public string DrDate { get; set; }

        [XmlElement(ElementName = "swbebtradeg")]
        public OrderGoodsList orderGoodsList { get; set; }


    }
}
