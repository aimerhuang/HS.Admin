using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class IpLookupResponse : JdObject{


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("data")]
public  		string
  data { get; set; }


}
}
