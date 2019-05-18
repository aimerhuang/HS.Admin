using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CouponDetail : JdObject{


         [XmlElement("orderId")]
public  		string
  orderId { get; set; }


         [XmlElement("skuId")]
public  		string
  skuId { get; set; }


         [XmlElement("couponType")]
public  		string
  couponType { get; set; }


         [XmlElement("couponPrice")]
public  		string
  couponPrice { get; set; }


}
}
