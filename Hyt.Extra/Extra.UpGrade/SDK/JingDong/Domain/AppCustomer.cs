using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AppCustomer : JdObject{


         [XmlElement("pin")]
public  		string
  pin { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("type")]
public  		string
  type { get; set; }


         [XmlElement("createDate")]
public  		string
  createDate { get; set; }


         [XmlElement("modified")]
public  		string
  modified { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("subscriptions")]
public  		string
  subscriptions { get; set; }


}
}
