using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ReportInfo : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("pin")]
public  		string
  pin { get; set; }


         [XmlElement("dimension")]
public  		string
  dimension { get; set; }


         [XmlElement("date")]
public  		DateTime
  date { get; set; }


         [XmlElement("figureData")]
public  		string
  figureData { get; set; }


}
}
