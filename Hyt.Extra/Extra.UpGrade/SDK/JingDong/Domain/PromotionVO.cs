using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PromotionVO : JdObject{


         [XmlElement("promo_id")]
public  		long
  promoId { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("type")]
public  		int
  type { get; set; }


         [XmlElement("bound")]
public  		int
  bound { get; set; }


         [XmlElement("begin_time")]
public  		string
  beginTime { get; set; }


         [XmlElement("end_time")]
public  		string
  endTime { get; set; }


         [XmlElement("member")]
public  		int
  member { get; set; }


         [XmlElement("slogan")]
public  		string
  slogan { get; set; }


         [XmlElement("comment")]
public  		string
  comment { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("favor_mode")]
public  		int
  favorMode { get; set; }


}
}
