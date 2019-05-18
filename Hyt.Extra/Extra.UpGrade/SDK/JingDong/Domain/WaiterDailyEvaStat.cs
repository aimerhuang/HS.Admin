using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WaiterDailyEvaStat : JdObject{


         [XmlElement("date")]
public  		string
  date { get; set; }


         [XmlElement("waiter")]
public  		string
  waiter { get; set; }


         [XmlElement("score")]
public  		string
  score { get; set; }


         [XmlElement("count")]
public  		string
  count { get; set; }


}
}
