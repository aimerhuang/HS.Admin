using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Coupon : JdObject{


         [XmlElement("type")]
public  		string
  type { get; set; }


         [XmlElement("couponPrice")]
public  		string
  couponPrice { get; set; }


}
}
