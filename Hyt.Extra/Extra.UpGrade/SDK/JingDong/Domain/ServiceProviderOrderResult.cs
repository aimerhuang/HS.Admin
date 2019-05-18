using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceProviderOrderResult : JdObject{


         [XmlElement("header")]
public  		string
  header { get; set; }


         [XmlElement("body")]
public  		List<string>
  body { get; set; }


}
}
