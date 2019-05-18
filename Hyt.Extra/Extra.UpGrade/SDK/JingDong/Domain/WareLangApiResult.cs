using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WareLangApiResult : JdObject{


         [XmlElement("wareId")]
public  		long
  wareId { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


         [XmlElement("langId")]
public  		int
  langId { get; set; }


         [XmlElement("seoKeywords")]
public  		string
  seoKeywords { get; set; }


         [XmlElement("extPackInfo")]
public  		string
  extPackInfo { get; set; }


         [XmlElement("saleStatus")]
public  		int
  saleStatus { get; set; }


         [XmlElement("onSaleTime")]
public  		DateTime
  onSaleTime { get; set; }


         [XmlElement("offSaleTime")]
public  		DateTime
  offSaleTime { get; set; }


         [XmlElement("langName")]
public  		string
  langName { get; set; }


         [XmlElement("langCode")]
public  		string
  langCode { get; set; }


         [XmlElement("description")]
public  		string
  description { get; set; }


         [XmlElement("appDescription")]
public  		string
  appDescription { get; set; }


         [XmlElement("messegeCode")]
public  		string
  messegeCode { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("success")]
public  		string
  success { get; set; }


}
}
