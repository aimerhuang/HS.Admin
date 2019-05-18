using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SellerPromotionGetResponse : JdResponse{


         [XmlElement("promotion_v_o")]
public  		string
  promotionVO { get; set; }


}
}
