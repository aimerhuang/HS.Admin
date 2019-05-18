using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class UserCategory3InfoDto : JdObject{


         [XmlElement("providerCode")]
public  		string
  providerCode { get; set; }


         [XmlElement("userCategory3Dtos")]
public  		List<string>
  userCategory3Dtos { get; set; }


}
}
