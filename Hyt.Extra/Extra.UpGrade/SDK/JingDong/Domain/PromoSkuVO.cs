using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PromoSkuVO : JdObject{


         [XmlElement("ware_id")]
public  		long
  wareId { get; set; }


         [XmlElement("item_num")]
public  		string
  itemNum { get; set; }


         [XmlElement("sku_id")]
public  		long
  skuId { get; set; }


         [XmlElement("sku_name")]
public  		string
  skuName { get; set; }


         [XmlElement("promo_id")]
public  		long
  promoId { get; set; }


         [XmlElement("jd_price")]
public  		string
  jdPrice { get; set; }


         [XmlElement("promo_price")]
public  		string
  promoPrice { get; set; }


         [XmlElement("seq")]
public  		int
  seq { get; set; }


         [XmlElement("num")]
public  		int
  num { get; set; }


         [XmlElement("bind_type")]
public  		int
  bindType { get; set; }


}
}
