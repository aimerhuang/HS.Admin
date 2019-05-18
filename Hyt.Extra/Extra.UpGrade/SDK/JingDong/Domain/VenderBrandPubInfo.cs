using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VenderBrandPubInfo : JdObject{


         [XmlElement("erpBrandId")]
public  		int
  erpBrandId { get; set; }


         [XmlElement("brandName")]
public  		string
  brandName { get; set; }


}
}
