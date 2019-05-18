using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SkuStock : JdObject{


         [XmlElement("detailStock")]
public  		string
  detailStock { get; set; }


         [XmlElement("skuId")]
public  		long
  skuId { get; set; }


         [XmlElement("stockNum")]
public  		long
  stockNum { get; set; }


         [XmlElement("storeId")]
public  		long
  storeId { get; set; }


}
}
