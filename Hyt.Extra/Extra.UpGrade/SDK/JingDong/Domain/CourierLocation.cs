using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CourierLocation : JdObject{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("gps")]
public  		string
  gps { get; set; }


}
}
