using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class KeywordTeam : JdObject{


         [XmlElement("beginTime")]
public  		long
  beginTime { get; set; }


         [XmlElement("cityId")]
public  		long
  cityId { get; set; }


         [XmlElement("cityName")]
public  		string
  cityName { get; set; }


         [XmlElement("endTime")]
public  		long
  endTime { get; set; }


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("image")]
public  		string
  image { get; set; }


         [XmlElement("marketPrice")]
public  		double
  marketPrice { get; set; }


         [XmlElement("maxNumber")]
public  		int
  maxNumber { get; set; }


         [XmlElement("minNumber")]
public  		int
  minNumber { get; set; }


         [XmlElement("nowNumber")]
public  		int
  nowNumber { get; set; }


         [XmlElement("sortOrder")]
public  		int
  sortOrder { get; set; }


         [XmlElement("externalUrl")]
public  		string
  externalUrl { get; set; }


         [XmlElement("teamPrice")]
public  		double
  teamPrice { get; set; }


         [XmlElement("teamType")]
public  		string
  teamType { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


         [XmlElement("credit")]
public  		int
  credit { get; set; }


         [XmlElement("feature")]
public  		int
  feature { get; set; }


}
}
