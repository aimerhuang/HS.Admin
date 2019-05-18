using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AsPromoInfoResponse : JdObject{


         [XmlElement("skuId")]
public  		string
  skuId { get; set; }


         [XmlElement("promoInfo")]
public  		List<string>
  promoInfo { get; set; }


}
}
