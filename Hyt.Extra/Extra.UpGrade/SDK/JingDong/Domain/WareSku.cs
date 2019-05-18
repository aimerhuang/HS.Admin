using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WareSku : JdObject{


         [XmlElement("skuId")]
public  		long
  skuId { get; set; }


         [XmlElement("wareId")]
public  		long
  wareId { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("attributes")]
public  		string
  attributes { get; set; }


         [XmlElement("supplyPrice")]
public  		double
  supplyPrice { get; set; }


         [XmlElement("stock")]
public  		int
  stock { get; set; }


         [XmlElement("imgUri")]
public  		string
  imgUri { get; set; }


         [XmlElement("hsCode")]
public  		string
  hsCode { get; set; }


         [XmlElement("amountCount")]
public  		int
  amountCount { get; set; }


         [XmlElement("lockCount")]
public  		int
  lockCount { get; set; }


         [XmlElement("lockStartTime")]
public  		DateTime
  lockStartTime { get; set; }


         [XmlElement("lockEndTime")]
public  		DateTime
  lockEndTime { get; set; }


         [XmlElement("saleStockAmount")]
public  		int
  saleStockAmount { get; set; }


}
}
