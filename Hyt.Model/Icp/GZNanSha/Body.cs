using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha
{
    public class Body
    {
        [XmlElement(ElementName = "GOODSRECORD")]
        public GOODSRECORD goodSrecord { get; set; }
    }
}
