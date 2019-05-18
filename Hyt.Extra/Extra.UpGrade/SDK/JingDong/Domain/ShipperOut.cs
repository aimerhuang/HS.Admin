using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ShipperOut : JdObject{


         [XmlElement("shipperNo")]
public  		string
  shipperNo { get; set; }


         [XmlElement("shipperName")]
public  		string
  shipperName { get; set; }


         [XmlElement("contacts")]
public  		string
  contacts { get; set; }


         [XmlElement("phone")]
public  		string
  phone { get; set; }


         [XmlElement("reserve1")]
public  		string
  reserve1 { get; set; }


         [XmlElement("reserve2")]
public  		string
  reserve2 { get; set; }


         [XmlElement("reserve3")]
public  		string
  reserve3 { get; set; }


         [XmlElement("reserve4")]
public  		string
  reserve4 { get; set; }


         [XmlElement("reserve5")]
public  		string
  reserve5 { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("type")]
public  		string
  type { get; set; }


         [XmlElement("isCod")]
public  		string
  isCod { get; set; }


         [XmlElement("isTemplate")]
public  		string
  isTemplate { get; set; }


}
}
