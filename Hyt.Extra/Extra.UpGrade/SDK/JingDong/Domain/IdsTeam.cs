using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class IdsTeam : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("cityId")]
public  		long
  cityId { get; set; }


         [XmlElement("cityName")]
public  		string
  cityName { get; set; }


         [XmlElement("groupId")]
public  		long
  groupId { get; set; }


         [XmlElement("groupName")]
public  		string
  groupName { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


         [XmlElement("product")]
public  		string
  product { get; set; }


         [XmlElement("marketPrice")]
public  		double
  marketPrice { get; set; }


         [XmlElement("teamPrice")]
public  		double
  teamPrice { get; set; }


         [XmlElement("beginTime")]
public  		long
  beginTime { get; set; }


         [XmlElement("endTime")]
public  		long
  endTime { get; set; }


         [XmlElement("couponExpireTime")]
public  		long
  couponExpireTime { get; set; }


         [XmlElement("minNumber")]
public  		int
  minNumber { get; set; }


         [XmlElement("maxNumber")]
public  		int
  maxNumber { get; set; }


         [XmlElement("perNumber")]
public  		int
  perNumber { get; set; }


         [XmlElement("nowNumber")]
public  		int
  nowNumber { get; set; }


         [XmlElement("teamImage")]
public  		string
  teamImage { get; set; }


         [XmlElement("externalUrl")]
public  		string
  externalUrl { get; set; }


         [XmlElement("teamType")]
public  		string
  teamType { get; set; }


         [XmlElement("detail")]
public  		string
  detail { get; set; }


         [XmlElement("redirectOption")]
public  		int
  redirectOption { get; set; }


}
}
