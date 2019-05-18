using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SellerPromotionV2GetPromoLimitResponse : JdResponse{


         [XmlElement("jos_promo_limit")]
public  		string
  josPromoLimit { get; set; }


}
}
