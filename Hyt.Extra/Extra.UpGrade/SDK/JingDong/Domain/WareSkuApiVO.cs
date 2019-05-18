using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WareSkuApiVO : JdObject{


         [XmlElement("skuId")]
public  		long
  skuId { get; set; }


         [XmlElement("wareId")]
public  		long
  wareId { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("rfId")]
public  		string
  rfId { get; set; }


         [XmlElement("attributes")]
public  		string
  attributes { get; set; }


         [XmlElement("supplyPrice")]
public  		string
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


}
}
