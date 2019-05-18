using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosPromotionSku : JdObject{


         [XmlElement("promo_sku_id")]
public  		long
  promoSkuId { get; set; }


         [XmlElement("ware_id")]
public  		long
  wareId { get; set; }


         [XmlElement("sku_id")]
public  		long
  skuId { get; set; }


         [XmlElement("sku_name")]
public  		string
  skuName { get; set; }


         [XmlElement("bind_type")]
public  		int
  bindType { get; set; }


         [XmlElement("jd_price")]
public  		string
  jdPrice { get; set; }


         [XmlElement("promo_price")]
public  		string
  promoPrice { get; set; }


         [XmlElement("item_num")]
public  		string
  itemNum { get; set; }


         [XmlElement("limit_num")]
public  		int
  limitNum { get; set; }


         [XmlElement("sku_status")]
public  		int
  skuStatus { get; set; }


         [XmlElement("seq")]
public  		int
  seq { get; set; }


         [XmlElement("display")]
public  		int
  display { get; set; }


         [XmlElement("is_need_to_buy")]
public  		int
  isNeedToBuy { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


}
}
