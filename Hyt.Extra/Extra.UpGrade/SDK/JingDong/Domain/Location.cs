using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Location : JdObject{


         [XmlElement("country")]
public  		string
  country { get; set; }


         [XmlElement("province")]
public  		string
  province { get; set; }


         [XmlElement("city")]
public  		string
  city { get; set; }


         [XmlElement("district")]
public  		string
  district { get; set; }


         [XmlElement("subdistrict")]
public  		string
  subdistrict { get; set; }


         [XmlElement("community")]
public  		string
  community { get; set; }


         [XmlElement("road")]
public  		string
  road { get; set; }


         [XmlElement("desc")]
public  		string
  desc { get; set; }


}
}
