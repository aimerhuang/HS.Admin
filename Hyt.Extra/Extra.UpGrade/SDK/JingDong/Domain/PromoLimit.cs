using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PromoLimit : JdObject{


         [XmlElement("vender_id")]
public  		long
  venderId { get; set; }


         [XmlElement("category_id")]
public  		long
  categoryId { get; set; }


         [XmlElement("discount_limit")]
public  		double
  discountLimit { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


}
}
