using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha.CustomsResult.Commodity
{
    public class Declaration
    {
        [XmlElement(ElementName = "OrgMessageID")]
        public string OrgMessageID { get; set; }

        [XmlElement(ElementName = "OrgMessageType")]
        public string OrgMessageType { get; set; }

        [XmlElement(ElementName = "OrgSenderID")]
        public string OrgSenderID { get; set; }

        [XmlElement(ElementName = "OrgReceiverID")]
        public string OrgReceiverID { get; set; }

        [XmlElement(ElementName = "OrgRecTime")]
        public string OrgRecTime { get; set; }

        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }

        [XmlElement(ElementName = "Notes")]
        public string Notes { get; set; }
    }
}
