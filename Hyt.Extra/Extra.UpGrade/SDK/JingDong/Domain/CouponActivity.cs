using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CouponActivity : JdObject{


         [XmlElement("activity_id")]
public  		long
  activityId { get; set; }


         [XmlElement("activity_name")]
public  		string
  activityName { get; set; }


         [XmlElement("activity_state")]
public  		int
  activityState { get; set; }


         [XmlElement("coupon_amount")]
public  		int
  couponAmount { get; set; }


         [XmlElement("coupon_sent_amcount")]
public  		int
  couponSentAmcount { get; set; }


         [XmlElement("coupon_used_amcount")]
public  		int
  couponUsedAmcount { get; set; }


         [XmlElement("coupon_id")]
public  		long
  couponId { get; set; }


         [XmlElement("send_time")]
public  		DateTime
  sendTime { get; set; }


         [XmlElement("start_time")]
public  		DateTime
  startTime { get; set; }


         [XmlElement("end_time")]
public  		DateTime
  endTime { get; set; }


}
}
