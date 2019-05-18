using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Subscription : JdObject{


         [XmlElement("group")]
public  		string
  group { get; set; }


         [XmlElement("topic")]
public  		string
  topic { get; set; }


}
}
