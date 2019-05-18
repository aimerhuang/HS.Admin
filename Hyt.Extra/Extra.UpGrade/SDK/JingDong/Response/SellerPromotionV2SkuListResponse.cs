using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SellerPromotionV2SkuListResponse : JdResponse{


         [XmlElement("promotion_sku_list")]
public  		List<string>
  promotionSkuList { get; set; }


}
}
