using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CourierListLocation : JdObject{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("arrivedTime")]
public  		DateTime
  arrivedTime { get; set; }


         [XmlElement("gpsList")]
public  		List<string>
  gpsList { get; set; }


}
}
