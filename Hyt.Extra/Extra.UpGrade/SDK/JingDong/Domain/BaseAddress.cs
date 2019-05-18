using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class BaseAddress : JdObject{


         [XmlElement("addressId")]
public  		string
  addressId { get; set; }


         [XmlElement("addressName")]
public  		string
  addressName { get; set; }


         [XmlElement("addressLevel")]
public  		int
  addressLevel { get; set; }


}
}
