using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StandardResponse : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("resultMsg")]
public  		string
  resultMsg { get; set; }


         [XmlElement("resultMap")]
public  		string
  resultMap { get; set; }


}
}
