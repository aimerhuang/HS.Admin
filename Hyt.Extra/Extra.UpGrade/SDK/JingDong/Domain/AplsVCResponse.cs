using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AplsVCResponse : JdObject{


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("errorType")]
public  		string
  errorType { get; set; }


         [XmlElement("errorCode")]
public  		string
  errorCode { get; set; }


         [XmlElement("success")]
public  		bool
  success { get; set; }


}
}
