using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





    [Serializable]
    public class AfsFreightOut : JdObject
    {


        [XmlElement("afsFreightId")]
        public int
          afsFreightId { get; set; }


        [XmlElement("afsServiceId")]
        public int
          afsServiceId { get; set; }


        [XmlElement("partReceiveId")]
        public int
          partReceiveId { get; set; }


        [XmlElement("freightCode")]
        public string
          freightCode { get; set; }


        [XmlElement("expressCode")]
        public string
          expressCode { get; set; }


        [XmlElement("freightMoney")]
        public string
          freightMoney { get; set; }


        [XmlElement("modifiedMoney")]
        public string
          modifiedMoney { get; set; }


        [XmlElement("expressCompany")]
        public string
          expressCompany { get; set; }


        [XmlElement("remark")]
        public string
          remark { get; set; }


    }
}
