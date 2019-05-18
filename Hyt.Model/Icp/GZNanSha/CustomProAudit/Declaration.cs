using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha.CustomProAudit
{
    public class Declaration
    {
        [XmlElement(ElementName = "GoodsRegRecList")]
        public List<Record> GoodsRegRecList { get; set; }
    }
}
