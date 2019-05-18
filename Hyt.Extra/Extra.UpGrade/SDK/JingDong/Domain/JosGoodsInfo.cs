using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosGoodsInfo : JdObject{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("wp_name")]
public  		string
  wpName { get; set; }


         [XmlElement("image_url")]
public  		string
  imageUrl { get; set; }


         [XmlElement("w_name")]
public  		string
  wName { get; set; }


         [XmlElement("wp_id")]
public  		string
  wpId { get; set; }


         [XmlElement("class_names")]
public  		string
  classNames { get; set; }


         [XmlElement("class_ids")]
public  		int
  classIds { get; set; }


         [XmlElement("image_urls")]
public  		string
  imageUrls { get; set; }


         [XmlElement("sku_similars")]
public  		List<string>
  skuSimilars { get; set; }


}
}
