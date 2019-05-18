using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SellerCouponReadGetCouponListResponse : JdResponse{


         [XmlElement("couponList")]
public  		List<string>
  couponList { get; set; }


}
}
