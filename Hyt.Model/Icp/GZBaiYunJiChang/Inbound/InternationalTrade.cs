using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Inbound
{
    public class InternationalTrade
    {
        [XmlElement(ElementName = "Head")]
        public Head Head { get; set; }
        [XmlElement(ElementName = "Declaration")]
        public Declaration Declaration { get; set; }
    }
}
