using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceProviderResultHead : JdObject{


         [XmlElement("resultStatus")]
public  		string
  resultStatus { get; set; }


         [XmlElement("resultMsg")]
public  		string
  resultMsg { get; set; }


}
}
