using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Inbound
{
    public class Declaration
    {
        [XmlElement(ElementName = "InboundHead")]
        public InboundHead InboundHead { get; set; }

        [XmlElement(ElementName = "InboundList")]
        public InboundList InboundList { get; set; }
    }
}
