using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OGoodsInfo : JdObject{


         [XmlElement("skuId")]
public  		string
  skuId { get; set; }


         [XmlElement("skuName")]
public  		string
  skuName { get; set; }


         [XmlElement("categoryId")]
public  		string
  categoryId { get; set; }


         [XmlElement("categoryName")]
public  		string
  categoryName { get; set; }


         [XmlElement("brandId")]
public  		string
  brandId { get; set; }


         [XmlElement("brandName")]
public  		string
  brandName { get; set; }


         [XmlElement("model")]
public  		string
  model { get; set; }


         [XmlElement("color")]
public  		string
  color { get; set; }


         [XmlElement("isSerial")]
public  		string
  isSerial { get; set; }


         [XmlElement("isStock")]
public  		string
  isStock { get; set; }


         [XmlElement("isYouShelfLife")]
public  		string
  isYouShelfLife { get; set; }


         [XmlElement("state")]
public  		string
  state { get; set; }


         [XmlElement("isCheapGift")]
public  		string
  isCheapGift { get; set; }


}
}
