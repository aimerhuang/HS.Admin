using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha.CustomsResult.CommodityAudit
{
    public class GoodsRegRecList
    {
        [XmlElement(ElementName = "GoodsRegRecList")]
        public List<Record> RecordList { get; set; }
    }
}
