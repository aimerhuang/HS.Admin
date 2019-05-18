using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SellerPromotionPropListResponse : JdResponse{


         [XmlElement("promo_prop_v_o_s")]
public  		List<string>
  promoPropVOS { get; set; }


}
}
