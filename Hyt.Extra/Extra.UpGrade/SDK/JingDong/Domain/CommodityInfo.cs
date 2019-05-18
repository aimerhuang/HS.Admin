using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CommodityInfo : JdObject{


         [XmlElement("sku_id")]
public  		long
  skuId { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


         [XmlElement("original_title")]
public  		string
  originalTitle { get; set; }


         [XmlElement("material_url")]
public  		string
  materialUrl { get; set; }


         [XmlElement("target_url")]
public  		string
  targetUrl { get; set; }


         [XmlElement("price")]
public  		string
  price { get; set; }


         [XmlElement("material_label")]
public  		string
  materialLabel { get; set; }


         [XmlElement("material_spu")]
public  		List<string>
  materialSpu { get; set; }


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("plan_id")]
public  		long
  planId { get; set; }


         [XmlElement("space_id")]
public  		long
  spaceId { get; set; }


         [XmlElement("review_status")]
public  		int
  reviewStatus { get; set; }


         [XmlElement("review_mark")]
public  		string
  reviewMark { get; set; }


         [XmlElement("show_days")]
public  		int
  showDays { get; set; }


}
}
