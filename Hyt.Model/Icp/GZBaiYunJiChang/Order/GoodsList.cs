using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Order
{
    public class GoodsList
    {
        [XmlElement(ElementName = "OrderGoodsList")]
        public List<OrderGoodsList> OrderGoodsListList { get; set; }
    }
}
