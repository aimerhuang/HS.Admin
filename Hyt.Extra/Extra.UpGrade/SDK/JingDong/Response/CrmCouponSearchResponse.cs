using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class CrmCouponSearchResponse : JdResponse{


         [XmlElement("coupon_result")]
public  		string
  couponResult { get; set; }


}
}
