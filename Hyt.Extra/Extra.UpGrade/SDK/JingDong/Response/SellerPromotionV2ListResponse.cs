using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SellerPromotionV2ListResponse : JdResponse{


         [XmlElement("promotion_list")]
public  		List<string>
  promotionList { get; set; }


}
}
