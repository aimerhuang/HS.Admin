using System;
using System.Xml.Serialization;
using System.Collections.Generic;

						using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SellerPromotionListResponse : JdResponse{


         [XmlElement("total_count")]
public  		int
  totalCount { get; set; }


         [XmlElement("promotion_v_o_s")]
public  		List<string>
  promotionVOS { get; set; }


}
}
