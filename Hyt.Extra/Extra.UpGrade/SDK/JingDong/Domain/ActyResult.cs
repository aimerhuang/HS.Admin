using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ActyResult : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("registrationItems")]
public  		List<string>
  registrationItems { get; set; }


         [XmlElement("count")]
public  		int
  count { get; set; }


}
}
