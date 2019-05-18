using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class NearTeam : JdObject{


         [XmlElement("beginTime")]
public  		long
  beginTime { get; set; }


         [XmlElement("distance_m")]
public  		long
  distanceM { get; set; }


         [XmlElement("endTime")]
public  		long
  endTime { get; set; }


         [XmlElement("marketPrice")]
public  		double
  marketPrice { get; set; }


         [XmlElement("teamPrice")]
public  		double
  teamPrice { get; set; }


         [XmlElement("partnerAddress")]
public  		string
  partnerAddress { get; set; }


         [XmlElement("partnerLat")]
public  		double
  partnerLat { get; set; }


         [XmlElement("partnerLng")]
public  		double
  partnerLng { get; set; }


         [XmlElement("partnerPhone")]
public  		string
  partnerPhone { get; set; }


         [XmlElement("partnerTitle")]
public  		string
  partnerTitle { get; set; }


         [XmlElement("externalUrl")]
public  		string
  externalUrl { get; set; }


         [XmlElement("teamId")]
public  		long
  teamId { get; set; }


         [XmlElement("teamImage")]
public  		string
  teamImage { get; set; }


         [XmlElement("teamTitle")]
public  		string
  teamTitle { get; set; }


}
}
