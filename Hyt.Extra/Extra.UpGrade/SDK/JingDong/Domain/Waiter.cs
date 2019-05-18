using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Waiter : JdObject{


         [XmlElement("waiter")]
public  		string
  waiter { get; set; }


         [XmlElement("yn")]
public  		string
  yn { get; set; }


         [XmlElement("leader")]
public  		string
  leader { get; set; }


         [XmlElement("level")]
public  		string
  level { get; set; }


}
}
