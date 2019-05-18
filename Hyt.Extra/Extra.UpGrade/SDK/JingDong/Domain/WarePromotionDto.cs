using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WarePromotionDto : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("ware_id")]
public  		long
  wareId { get; set; }


         [XmlElement("ware_name")]
public  		string
  wareName { get; set; }


         [XmlElement("promotion_name")]
public  		string
  promotionName { get; set; }


         [XmlElement("channels")]
public  		string
  channels { get; set; }


         [XmlElement("fixed_price")]
public  		string
  fixedPrice { get; set; }


         [XmlElement("prom_advice_word")]
public  		string
  promAdviceWord { get; set; }


         [XmlElement("act_link_name")]
public  		string
  actLinkName { get; set; }


         [XmlElement("act_link_url")]
public  		string
  actLinkUrl { get; set; }


         [XmlElement("start_time")]
public  		DateTime
  startTime { get; set; }


         [XmlElement("end_time")]
public  		DateTime
  endTime { get; set; }


         [XmlElement("limit_num")]
public  		int
  limitNum { get; set; }


         [XmlElement("rebate_file")]
public  		string
  rebateFile { get; set; }


         [XmlElement("jd_price")]
public  		string
  jdPrice { get; set; }


         [XmlElement("down_discount")]
public  		string
  downDiscount { get; set; }


         [XmlElement("cbj_price")]
public  		string
  cbjPrice { get; set; }


         [XmlElement("discount_amount")]
public  		string
  discountAmount { get; set; }


         [XmlElement("grossmargin")]
public  		string
  grossmargin { get; set; }


         [XmlElement("error_message")]
public  		string
  errorMessage { get; set; }


         [XmlElement("over_lying_suit")]
public  		int
  overLyingSuit { get; set; }


         [XmlElement("sale_mode")]
public  		int
  saleMode { get; set; }


}
}
