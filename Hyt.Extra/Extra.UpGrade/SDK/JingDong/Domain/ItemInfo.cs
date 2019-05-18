using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ItemInfo : JdObject{


         [XmlElement("skuId")]
public  		string
  skuId { get; set; }


         [XmlElement("outerSkuId")]
public  		string
  outerSkuId { get; set; }


         [XmlElement("skuName")]
public  		string
  skuName { get; set; }


         [XmlElement("jdPrice")]
public  		string
  jdPrice { get; set; }


         [XmlElement("giftPoint")]
public  		string
  giftPoint { get; set; }


         [XmlElement("wareId")]
public  		string
  wareId { get; set; }


         [XmlElement("itemTotal")]
public  		string
  itemTotal { get; set; }


         [XmlElement("productNo")]
public  		string
  productNo { get; set; }


         [XmlElement("serviceName")]
public  		string
  serviceName { get; set; }


}
}
