using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosSalesReturnDto : JdObject{


         [XmlElement("vendorCode")]
public  		string
  vendorCode { get; set; }


         [XmlElement("serialNo")]
public  		string
  serialNo { get; set; }


         [XmlElement("rkBusId")]
public  		string
  rkBusId { get; set; }


         [XmlElement("rkTime")]
public  		DateTime
  rkTime { get; set; }


}
}
