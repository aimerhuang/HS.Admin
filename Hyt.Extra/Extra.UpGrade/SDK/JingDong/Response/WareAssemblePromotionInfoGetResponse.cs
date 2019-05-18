using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WareAssemblePromotionInfoGetResponse : JdResponse{


         [XmlElement("promoInfoResponseList")]
public  		List<string>
  promoInfoResponseList { get; set; }


}
}
