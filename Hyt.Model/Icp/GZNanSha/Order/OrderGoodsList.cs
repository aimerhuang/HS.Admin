using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha.Order
{
    public class OrderGoodsList
    {
        [XmlElement(ElementName = "Record")]
        public List<CustomOrderGoodsMod> list = new List<CustomOrderGoodsMod>();
    }
}
