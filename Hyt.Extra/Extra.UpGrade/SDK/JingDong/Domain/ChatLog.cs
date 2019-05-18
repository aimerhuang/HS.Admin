using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ChatLog : JdObject{


         [XmlElement("customer")]
public  		string
  customer { get; set; }


         [XmlElement("waiter")]
public  		string
  waiter { get; set; }


         [XmlElement("content")]
public  		string
  content { get; set; }


         [XmlElement("sid")]
public  		string
  sid { get; set; }


         [XmlElement("skuId")]
public  		long
  skuId { get; set; }


         [XmlElement("time")]
public  		DateTime
  time { get; set; }


         [XmlElement("channel")]
public  		string
  channel { get; set; }


         [XmlElement("waiterSend")]
public  		string
  waiterSend { get; set; }


}
}
