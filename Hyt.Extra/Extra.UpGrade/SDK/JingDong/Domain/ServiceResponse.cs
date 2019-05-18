using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceResponse : JdObject{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("msg")]
public  		string
  msg { get; set; }


}
}
