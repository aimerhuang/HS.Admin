using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ExternalVerOrder : JdObject{


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("wangUser")]
public  		string
  wangUser { get; set; }


         [XmlElement("wangUserPhone")]
public  		string
  wangUserPhone { get; set; }


         [XmlElement("verDate")]
public  		DateTime
  verDate { get; set; }


         [XmlElement("verStatus")]
public  		int
  verStatus { get; set; }


         [XmlElement("img")]
public  		string
  img { get; set; }


}
}
