using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class BooleanDto : JdObject{


         [XmlElement("data")]
public  		bool
  data { get; set; }


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


}
}
