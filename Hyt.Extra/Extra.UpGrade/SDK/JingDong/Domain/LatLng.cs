using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class LatLng : JdObject{


         [XmlElement("lat")]
public  		double
  lat { get; set; }


         [XmlElement("lng")]
public  		double
  lng { get; set; }


}
}
