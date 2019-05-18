using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AddressResponse : JdObject{


         [XmlElement("address")]
public  		List<string>
  address { get; set; }


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


}
}
