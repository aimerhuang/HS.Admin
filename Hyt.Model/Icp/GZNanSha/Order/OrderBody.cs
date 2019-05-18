using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha.Order
{
    public class OrderBody
    {
        [XmlElement(ElementName = "swbebtrade")]
        public OrderBodyRecord record { get; set; }
    }

    public class OrderBodyRecord
    {
        [XmlElement(ElementName = "Record")]
        public CustomOrderMod cusOrderMod { get; set; }
    }
}
