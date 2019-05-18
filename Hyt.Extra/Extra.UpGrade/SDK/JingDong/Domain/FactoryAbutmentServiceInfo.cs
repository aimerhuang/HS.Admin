using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class FactoryAbutmentServiceInfo : JdObject{


         [XmlElement("orderno")]
public  		string
  orderno { get; set; }


         [XmlElement("serviceTypeId")]
public  		int
  serviceTypeId { get; set; }


         [XmlElement("serviceTypeName")]
public  		string
  serviceTypeName { get; set; }


}
}
