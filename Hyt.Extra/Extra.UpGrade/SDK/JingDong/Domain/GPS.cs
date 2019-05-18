using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class GPS : JdObject{


         [XmlElement("gpsTime")]
public  		DateTime
  gpsTime { get; set; }


         [XmlElement("lat")]
public  		double
  lat { get; set; }


         [XmlElement("lng")]
public  		double
  lng { get; set; }


}
}
