using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods
{
    public class GoodsRegList
    {
        [XmlElement(ElementName = "GoodsContent")]
        public List<GoodsContent> GoodsContentList { get; set; }
    }
}
