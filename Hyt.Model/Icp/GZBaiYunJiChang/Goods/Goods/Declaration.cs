using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods
{
    public class Declaration
    {
        [XmlElement(ElementName = "GoodsRegHead")]
        public GoodsRegHead GoodsRegHead { get; set; }

        [XmlElement(ElementName = "GoodsRegList")]
        public GoodsRegList GoodsRegList { get; set; }
    }
}
