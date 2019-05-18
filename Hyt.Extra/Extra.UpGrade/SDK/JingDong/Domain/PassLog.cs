using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PassLog : JdObject{


         [XmlElement("waiter")]
public  		string
  waiter { get; set; }


         [XmlElement("loginTime")]
public  		DateTime
  loginTime { get; set; }


         [XmlElement("logoutTime")]
public  		DateTime
  logoutTime { get; set; }


         [XmlElement("ip")]
public  		string
  ip { get; set; }


         [XmlElement("loginSid")]
public  		string
  loginSid { get; set; }


}
}
