using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class CrmCouponActivitySearchResponse : JdResponse{


         [XmlElement("coupon_activity_result")]
public  		string
  couponActivityResult { get; set; }


}
}
