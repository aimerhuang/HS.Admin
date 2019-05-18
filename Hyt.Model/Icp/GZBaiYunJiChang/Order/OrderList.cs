using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Order
{
    public class OrderList
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        [XmlElement(ElementName = "OrderContent")]
        public List<OrderContent> OrderContentList { get; set; }
    }
}
