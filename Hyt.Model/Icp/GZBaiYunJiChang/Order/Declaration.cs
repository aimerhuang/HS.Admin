using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Order
{
    public class Declaration
    {
        [XmlElement(ElementName = "OrderHead")]
        public OrderHead OrderHead { get; set; }

        [XmlElement(ElementName = "OrderList")]
        public OrderList OrderList { get; set; }
    }
}
