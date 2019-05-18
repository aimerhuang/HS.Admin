using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SellerPromotionOrdermodeListResponse : JdResponse{


         [XmlElement("promo_order_mode_v_os")]
public  		List<string>
  promoOrderModeVOs { get; set; }


}
}
