using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Inbound
{
    public class InboundList
    {
        [XmlElement(ElementName = "InboundContent")]
        public List<InboundContent> InboundContentList { get; set; }
    }
}
