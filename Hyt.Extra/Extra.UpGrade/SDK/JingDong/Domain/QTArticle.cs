using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QTArticle : JdObject{


         [XmlElement("service_item_code")]
public  		string
  serviceItemCode { get; set; }


         [XmlElement("pin")]
public  		string
  pin { get; set; }


         [XmlElement("all_num")]
public  		int
  allNum { get; set; }


         [XmlElement("used_num")]
public  		int
  usedNum { get; set; }


         [XmlElement("future_price")]
public  		string
  futurePrice { get; set; }


         [XmlElement("future_sub_id")]
public  		int
  futureSubId { get; set; }


         [XmlElement("expire_time")]
public  		DateTime
  expireTime { get; set; }


         [XmlElement("usage_details")]
public  		List<string>
  usageDetails { get; set; }


}
}
