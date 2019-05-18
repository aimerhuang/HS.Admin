using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class LocationDto : JdObject{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("data")]
public  		string
  data { get; set; }


}
}
