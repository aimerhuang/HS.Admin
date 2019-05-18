using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha.CustomProAudit
{
    public class Record
    {
        [XmlElement(ElementName = "CargoBcode")]
        public string CargoBcode { get; set; }

        [XmlElement(ElementName = "CbeComcode")]
        public string CbeComcode { get; set; }

        [XmlElement(ElementName = "Gcode")]
        public string Gcode { get; set; }

        [XmlElement(ElementName = "CIQGoodsNO")]
        public string CIQGoodsNO { get; set; }

        [XmlElement(ElementName = "RegStatus")]
        public string RegStatus { get; set; }

        [XmlElement(ElementName = "RegNotes")]
        public string RegNotes { get; set; }
    }
}
