using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha
{
    public class GOODSRECORD
    {
        [XmlElement(ElementName = "Record")]
        public Record record { get; set; }
    }
}
