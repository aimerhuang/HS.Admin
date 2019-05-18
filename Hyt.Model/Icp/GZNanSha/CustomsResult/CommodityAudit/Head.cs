using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha.CustomsResult.CommodityAudit
{
    public class Head
    {
        [XmlElement(ElementName = "MessageID")]
        public string MessageID { get; set; }

        [XmlElement(ElementName = "MessageType")]
        public string MessageType { get; set; }

        [XmlElement(ElementName = "Sender")]
        public string Sender { get; set; }

        [XmlElement(ElementName = "Receiver")]
        public string Receiver { get; set; }

        [XmlElement(ElementName = "SendTime")]
        public string SendTime { get; set; }

        [XmlElement(ElementName = "FunctionCode")]
        public string FunctionCode { get; set; }

        [XmlElement(ElementName = "Version")]
        public string Version { get; set; }
    }
}
