using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceResult : JdObject{


         [XmlElement("success")]
public  		string
  success { get; set; }


         [XmlElement("service")]
public  		string
  service { get; set; }


}
}
