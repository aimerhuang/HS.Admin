using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServicesResult : JdObject{


         [XmlElement("success")]
public  		string
  success { get; set; }


         [XmlElement("services")]
public  		List<string>
  services { get; set; }


}
}
