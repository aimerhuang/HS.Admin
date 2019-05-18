using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WaiterDailyStat : JdObject{


         [XmlElement("date")]
public  		string
  date { get; set; }


         [XmlElement("waiter")]
public  		string
  waiter { get; set; }


         [XmlElement("result")]
public  		string
  result { get; set; }


}
}
