using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosSkuInfo : JdObject{


         [XmlElement("sku_id")]
public  		string
  skuId { get; set; }


         [XmlElement("sku_url")]
public  		string
  skuUrl { get; set; }


}
}
