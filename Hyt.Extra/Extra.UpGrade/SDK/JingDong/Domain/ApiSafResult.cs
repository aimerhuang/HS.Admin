using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ApiSafResult : JdObject{


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("resultCode")]
public  		string
  resultCode { get; set; }


         [XmlElement("resultDescribe")]
public  		string
  resultDescribe { get; set; }


}
}
