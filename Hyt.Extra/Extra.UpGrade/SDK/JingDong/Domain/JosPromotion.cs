using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosPromotion : JdObject{


         [XmlElement("vender_id")]
public  		long
  venderId { get; set; }


         [XmlElement("promo_id")]
public  		long
  promoId { get; set; }


         [XmlElement("promo_name")]
public  		string
  promoName { get; set; }


         [XmlElement("promo_type")]
public  		int
  promoType { get; set; }


         [XmlElement("favor_mode")]
public  		int
  favorMode { get; set; }


         [XmlElement("begin_time")]
public  		string
  beginTime { get; set; }


         [XmlElement("end_time")]
public  		string
  endTime { get; set; }


         [XmlElement("bound")]
public  		int
  bound { get; set; }


         [XmlElement("member")]
public  		int
  member { get; set; }


         [XmlElement("slogan")]
public  		string
  slogan { get; set; }


         [XmlElement("comment")]
public  		string
  comment { get; set; }


         [XmlElement("promo_status")]
public  		int
  promoStatus { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


         [XmlElement("platform")]
public  		int
  platform { get; set; }


         [XmlElement("link")]
public  		string
  link { get; set; }


         [XmlElement("shop_member")]
public  		int
  shopMember { get; set; }


         [XmlElement("qq_member")]
public  		int
  qqMember { get; set; }


         [XmlElement("plus_member")]
public  		int
  plusMember { get; set; }


         [XmlElement("member_level_only")]
public  		string
  memberLevelOnly { get; set; }


         [XmlElement("allow_others_operate")]
public  		string
  allowOthersOperate { get; set; }


         [XmlElement("allow_others_check")]
public  		string
  allowOthersCheck { get; set; }


         [XmlElement("allow_other_user_operate")]
public  		string
  allowOtherUserOperate { get; set; }


         [XmlElement("allow_other_user_check")]
public  		string
  allowOtherUserCheck { get; set; }


         [XmlElement("need_manual_check")]
public  		string
  needManualCheck { get; set; }


         [XmlElement("allow_check")]
public  		string
  allowCheck { get; set; }


         [XmlElement("allow_operate")]
public  		string
  allowOperate { get; set; }


         [XmlElement("is_jingdou_required")]
public  		string
  isJingdouRequired { get; set; }


         [XmlElement("freq_bound")]
public  		int
  freqBound { get; set; }


         [XmlElement("per_max_num")]
public  		int
  perMaxNum { get; set; }


         [XmlElement("per_min_num")]
public  		int
  perMinNum { get; set; }


         [XmlElement("prop_type")]
public  		int
  propType { get; set; }


         [XmlElement("prop_num")]
public  		int
  propNum { get; set; }


         [XmlElement("prop_used_way")]
public  		int
  propUsedWay { get; set; }


         [XmlElement("coupon_id")]
public  		int
  couponId { get; set; }


         [XmlElement("coupon_batch_key")]
public  		string
  couponBatchKey { get; set; }


         [XmlElement("coupon_valid_days")]
public  		int
  couponValidDays { get; set; }


         [XmlElement("quota")]
public  		string
  quota { get; set; }


         [XmlElement("rate")]
public  		string
  rate { get; set; }


         [XmlElement("plus")]
public  		string
  plus { get; set; }


         [XmlElement("order_mode_desc")]
public  		string
  orderModeDesc { get; set; }


         [XmlElement("token_use_num")]
public  		int
  tokenUseNum { get; set; }


         [XmlElement("user_pins")]
public  		string
  userPins { get; set; }


         [XmlElement("promo_area_type")]
public  		int
  promoAreaType { get; set; }


         [XmlElement("promo_areas")]
public  		string
  promoAreas { get; set; }


}
}
