using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ProductInfo : JdObject{


         [XmlElement("skuId")]
public  		long
  skuId { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("amount")]
public  		int
  amount { get; set; }


         [XmlElement("price")]
public  		long
  price { get; set; }


}
}
