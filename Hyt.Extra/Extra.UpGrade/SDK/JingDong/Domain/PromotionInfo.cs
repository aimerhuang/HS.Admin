using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PromotionInfo : JdObject{


         [XmlElement("promoId")]
public  		string
  promoId { get; set; }


         [XmlElement("promoType")]
public  		int
  promoType { get; set; }


         [XmlElement("extType")]
public  		int
  extType { get; set; }


         [XmlElement("fullRefundType")]
public  		int
  fullRefundType { get; set; }


         [XmlElement("userLevel")]
public  		int
  userLevel { get; set; }


         [XmlElement("minNum")]
public  		int
  minNum { get; set; }


         [XmlElement("maxNum")]
public  		int
  maxNum { get; set; }


         [XmlElement("limitUserType")]
public  		int
  limitUserType { get; set; }


         [XmlElement("price")]
public  		string
  price { get; set; }


         [XmlElement("discount")]
public  		string
  discount { get; set; }


         [XmlElement("reward")]
public  		string
  reward { get; set; }


         [XmlElement("addMoney")]
public  		string
  addMoney { get; set; }


         [XmlElement("needMondey")]
public  		string
  needMondey { get; set; }


         [XmlElement("needNum")]
public  		int
  needNum { get; set; }


         [XmlElement("deliverNum")]
public  		int
  deliverNum { get; set; }


         [XmlElement("topMoney")]
public  		string
  topMoney { get; set; }


         [XmlElement("percent")]
public  		double
  percent { get; set; }


         [XmlElement("rebate")]
public  		double
  rebate { get; set; }


         [XmlElement("haveFullRefundGifts")]
public  		bool
  haveFullRefundGifts { get; set; }


         [XmlElement("score")]
public  		int
  score { get; set; }


         [XmlElement("promoEndTime")]
public  		long
  promoEndTime { get; set; }


         [XmlElement("adwordCouponList")]
public  		List<string>
  adwordCouponList { get; set; }


         [XmlElement("adwordGiftSkuList")]
public  		List<string>
  adwordGiftSkuList { get; set; }


}
}
