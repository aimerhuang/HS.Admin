using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ProviderDTO : JdObject{


         [XmlElement("id")]
public  		int
  id { get; set; }


         [XmlElement("providerCode")]
public  		string
  providerCode { get; set; }


         [XmlElement("providerName")]
public  		string
  providerName { get; set; }


         [XmlElement("providerType")]
public  		string
  providerType { get; set; }


         [XmlElement("operationType")]
public  		string
  operationType { get; set; }


         [XmlElement("rangeType")]
public  		string
  rangeType { get; set; }


         [XmlElement("contactName")]
public  		string
  contactName { get; set; }


         [XmlElement("contactPhone")]
public  		string
  contactPhone { get; set; }


         [XmlElement("contactMobile")]
public  		string
  contactMobile { get; set; }


         [XmlElement("inPlatform")]
public  		bool
  inPlatform { get; set; }


         [XmlElement("supportCod")]
public  		bool
  supportCod { get; set; }


         [XmlElement("approveState")]
public  		string
  approveState { get; set; }


         [XmlElement("approveComment")]
public  		string
  approveComment { get; set; }


         [XmlElement("providerState")]
public  		string
  providerState { get; set; }


}
}
