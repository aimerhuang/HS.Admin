using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SamSku : JdObject{


         [XmlElement("skuId")]
public  		long
  skuId { get; set; }


         [XmlElement("itemId")]
public  		string
  itemId { get; set; }


         [XmlElement("sequenceId")]
public  		string
  sequenceId { get; set; }


         [XmlElement("payPrice")]
public  		string
  payPrice { get; set; }


         [XmlElement("coupons")]
public  		List<string>
  coupons { get; set; }


}
}
