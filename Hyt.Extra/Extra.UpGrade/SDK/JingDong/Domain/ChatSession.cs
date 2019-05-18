using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ChatSession : JdObject{


         [XmlElement("customer")]
public  		string
  customer { get; set; }


         [XmlElement("waiter")]
public  		string
  waiter { get; set; }


         [XmlElement("beginTime")]
public  		DateTime
  beginTime { get; set; }


         [XmlElement("replyTime")]
public  		DateTime
  replyTime { get; set; }


         [XmlElement("endTime")]
public  		DateTime
  endTime { get; set; }


         [XmlElement("sessionType")]
public  		string
  sessionType { get; set; }


         [XmlElement("transfer")]
public  		string
  transfer { get; set; }


         [XmlElement("sid")]
public  		string
  sid { get; set; }


         [XmlElement("skuId")]
public  		int
  skuId { get; set; }


}
}
