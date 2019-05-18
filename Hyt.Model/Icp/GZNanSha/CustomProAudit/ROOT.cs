using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha.CustomProAudit
{
    public class ROOT
    {
        [XmlElement(ElementName = "Head")]
        public Hyt.Model.Icp.GZNanSha.CustomsResult.Commodity.Head head { get; set; }
        [XmlElement(ElementName = "Declaration")]
        public Declaration declaration { get; set; }
    }
}
