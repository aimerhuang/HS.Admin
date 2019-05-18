using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ShopJosResult : JdObject{


         [XmlElement("vender_id")]
public  		long
  venderId { get; set; }


         [XmlElement("shop_id")]
public  		long
  shopId { get; set; }


         [XmlElement("shop_name")]
public  		string
  shopName { get; set; }


         [XmlElement("open_time")]
public  		DateTime
  openTime { get; set; }


         [XmlElement("logo_url")]
public  		string
  logoUrl { get; set; }


         [XmlElement("brief")]
public  		string
  brief { get; set; }


         [XmlElement("category_main")]
public  		long
  categoryMain { get; set; }


         [XmlElement("category_main_name")]
public  		string
  categoryMainName { get; set; }


}
}
