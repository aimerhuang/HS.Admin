using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CouponResult : JdObject{


         [XmlElement("coupons")]
public  		string
  coupons { get; set; }


         [XmlElement("total_result")]
public  		string
  totalResult { get; set; }


}
}
