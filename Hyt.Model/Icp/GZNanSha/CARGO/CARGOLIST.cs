using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha.CARGO
{
    public class CARGOLIST
    {
        [XmlElement(ElementName = "Record")]
        public List<Record1> recordList { get; set; }
    }
}
