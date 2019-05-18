using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceExpressInfoExport : JdObject{


         [XmlElement("afsServiceId")]
public  		int
  afsServiceId { get; set; }


         [XmlElement("freightMoney")]
public  		string
  freightMoney { get; set; }


         [XmlElement("expressCompany")]
public  		string
  expressCompany { get; set; }


         [XmlElement("deliveryDate")]
public  		DateTime
  deliveryDate { get; set; }


         [XmlElement("createDate")]
public  		DateTime
  createDate { get; set; }


         [XmlElement("expressCode")]
public  		string
  expressCode { get; set; }


}
}
