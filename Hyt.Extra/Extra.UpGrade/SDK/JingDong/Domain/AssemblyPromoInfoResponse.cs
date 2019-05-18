using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AssemblyPromoInfoResponse : JdObject{


         [XmlElement("promoId")]
public  		string
  promoId { get; set; }


         [XmlElement("promoType")]
public  		int
  promoType { get; set; }


         [XmlElement("promoTag")]
public  		string
  promoTag { get; set; }


         [XmlElement("promoInfo")]
public  		string
  promoInfo { get; set; }


         [XmlElement("promoEndTime")]
public  		long
  promoEndTime { get; set; }


         [XmlElement("extProperty")]
public  		string
  extProperty { get; set; }


}
}
