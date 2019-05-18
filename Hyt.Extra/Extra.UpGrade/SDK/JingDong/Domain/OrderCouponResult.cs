using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderCouponResult : JdObject{


         [XmlElement("result")]
public  		string
  result { get; set; }


         [XmlElement("msgs")]
public  		string
  msgs { get; set; }


         [XmlElement("orderCoupons")]
public  		List<string>
  orderCoupons { get; set; }


}
}
