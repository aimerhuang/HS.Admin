using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha.CustomsResult.CommodityAudit
{
    public class ROOT
    {
        [XmlElement(ElementName = "Head")]
        public Head head { get; set; }
        [XmlElement(ElementName = "Declaration")]
        public Declaration declaration { get; set; }
    }
}
