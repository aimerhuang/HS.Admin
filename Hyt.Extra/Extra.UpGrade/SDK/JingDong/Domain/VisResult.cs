using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VisResult : JdObject{


         [XmlElement("flag")]
public  		string
  flag { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


}
}
