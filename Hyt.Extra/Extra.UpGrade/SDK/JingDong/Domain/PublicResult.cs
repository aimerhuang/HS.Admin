using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PublicResult : JdObject{


         [XmlElement("success")]
public  		string
  success { get; set; }


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("msg")]
public  		string
  msg { get; set; }


         [XmlElement("model")]
public  		string
  model { get; set; }


}
}
