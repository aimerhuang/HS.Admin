using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha.Order
{
    public class CustomOrderGoodsMod
    {
        [XmlElement(ElementName = "EntGoodsNo")]
        public string EntGoodsNo { get; set; }

        [XmlElement(ElementName = "Gcode")]
        public string Gcode { get; set; }

        [XmlElement(ElementName = "Hscode")]
        public string Hscode { get; set; }

        [XmlElement(ElementName = "CiqGoodsNo")]
        public string CiqGoodsNo { get; set; }

        [XmlElement(ElementName = "CopGName")]
        public string CopGName { get; set; }

        [XmlElement(ElementName = "Brand")]
        public string Brand { get; set; }

        [XmlElement(ElementName = "Spec")]
        public string Spec { get; set; }

        [XmlElement(ElementName = "Origin")]
        public string Origin { get; set; }

        [XmlElement(ElementName = "Qty")]
        public string Qty { get; set; }

        [XmlElement(ElementName = "QtyUnit")]
        public string QtyUnit { get; set; }

        [XmlElement(ElementName = "DecPrice")]
        public decimal DecPrice { get; set; }

        [XmlElement(ElementName = "DecTotal")]
        public decimal DecTotal { get; set; }

        [XmlElement(ElementName = "SellWebSite")]
        public string SellWebSite { get; set; }

        [XmlElement(ElementName = "Nots")]
        public string Nots { get; set; }


    }
}
