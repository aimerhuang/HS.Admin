using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha.CustomsResult.OrderResponse
{
    public class ResponseMessage
    {
        [XmlElement(ElementName = "SenderID")]
        public string SenderID { get; set; }

        [XmlElement(ElementName = "MessageType")]
        public string MessageType { get; set; }

        [XmlElement(ElementName = "MessageID")]
        public string MessageID { get; set; }

        [XmlElement(ElementName = "ReturnCode")]
        public string ReturnCode { get; set; }

        [XmlElement(ElementName = "ReturnInfo")]
        public string ReturnInfo { get; set; }

        [XmlElement(ElementName = "ReturnDate")]
        public string ReturnDate { get; set; }

        [XmlElement(ElementName = "FileName")]
        public string FileName { get; set; }
    }
}
