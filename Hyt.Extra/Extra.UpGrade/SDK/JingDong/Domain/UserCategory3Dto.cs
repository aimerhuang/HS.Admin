using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class UserCategory3Dto : JdObject{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


}
}
