using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Component : JdObject{


         [XmlElement("key")]
public  		string
  key { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


}
}
