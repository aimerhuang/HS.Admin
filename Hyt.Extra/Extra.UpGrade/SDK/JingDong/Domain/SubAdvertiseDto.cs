using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SubAdvertiseDto : JdObject{


         [XmlElement("sku_id")]
public  		string
  skuId { get; set; }


         [XmlElement("sku_name")]
public  		string
  skuName { get; set; }


         [XmlElement("brand_name")]
public  		string
  brandName { get; set; }


         [XmlElement("adword")]
public  		string
  adword { get; set; }


         [XmlElement("adword_old")]
public  		string
  adwordOld { get; set; }


}
}
