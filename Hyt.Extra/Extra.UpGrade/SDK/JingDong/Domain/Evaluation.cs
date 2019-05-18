using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Evaluation : JdObject{


         [XmlElement("customer")]
public  		string
  customer { get; set; }


         [XmlElement("waiter")]
public  		string
  waiter { get; set; }


         [XmlElement("desc")]
public  		string
  desc { get; set; }


         [XmlElement("score")]
public  		string
  score { get; set; }


         [XmlElement("evaTime")]
public  		DateTime
  evaTime { get; set; }


}
}
