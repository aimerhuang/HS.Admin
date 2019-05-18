using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Packs : JdObject{


         [XmlElement("packListPrice")]
public  		string
  packListPrice { get; set; }


         [XmlElement("packPrice")]
public  		string
  packPrice { get; set; }


         [XmlElement("mainSkuPicUrl")]
public  		string
  mainSkuPicUrl { get; set; }


         [XmlElement("packId")]
public  		string
  packId { get; set; }


         [XmlElement("mainSkuId")]
public  		string
  mainSkuId { get; set; }


         [XmlElement("discount")]
public  		string
  discount { get; set; }


         [XmlElement("mainSkuName")]
public  		string
  mainSkuName { get; set; }


         [XmlElement("packs")]
public  		List<string>
  packs { get; set; }


}
}
