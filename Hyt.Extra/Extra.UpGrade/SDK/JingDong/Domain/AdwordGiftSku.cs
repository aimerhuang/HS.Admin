using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AdwordGiftSku : JdObject{


         [XmlElement("skuId")]
public  		string
  skuId { get; set; }


         [XmlElement("imagePath")]
public  		string
  imagePath { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("number")]
public  		int
  number { get; set; }


         [XmlElement("giftType")]
public  		int
  giftType { get; set; }


         [XmlElement("giftState")]
public  		int
  giftState { get; set; }


}
}
