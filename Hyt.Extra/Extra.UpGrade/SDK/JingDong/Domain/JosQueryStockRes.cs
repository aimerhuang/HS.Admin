using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosQueryStockRes : JdObject{


         [XmlElement("spuId")]
public  		long
  spuId { get; set; }


         [XmlElement("skuId")]
public  		long
  skuId { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("amountCount")]
public  		int
  amountCount { get; set; }


         [XmlElement("avaiableStockAmount")]
public  		int
  avaiableStockAmount { get; set; }


}
}
