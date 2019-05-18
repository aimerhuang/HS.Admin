using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CustomClearanceResponse : JdObject{


         [XmlElement("resultCode")]
public  		string
  resultCode { get; set; }


         [XmlElement("resultMessage")]
public  		string
  resultMessage { get; set; }


}
}
