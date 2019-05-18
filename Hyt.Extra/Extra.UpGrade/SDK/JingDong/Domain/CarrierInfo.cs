using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CarrierInfo : JdObject{


         [XmlElement("tenantId")]
public  		string
  tenantId { get; set; }


         [XmlElement("carrierNo")]
public  		string
  carrierNo { get; set; }


         [XmlElement("carrierName")]
public  		string
  carrierName { get; set; }


         [XmlElement("tel")]
public  		string
  tel { get; set; }


         [XmlElement("useFlag")]
public  		string
  useFlag { get; set; }


         [XmlElement("contact")]
public  		string
  contact { get; set; }


         [XmlElement("postCode")]
public  		string
  postCode { get; set; }


         [XmlElement("carrierType")]
public  		int
  carrierType { get; set; }


         [XmlElement("address")]
public  		string
  address { get; set; }


}
}
