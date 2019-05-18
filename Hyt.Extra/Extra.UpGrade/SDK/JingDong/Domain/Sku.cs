using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Sku : JdObject{


         [XmlElement("wareId")]
public  		long
  wareId { get; set; }


         [XmlElement("skuId")]
public  		long
  skuId { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("saleAttrs")]
public  		string
  saleAttrs { get; set; }


         [XmlElement("features")]
public  		string
  features { get; set; }


         [XmlElement("jdPrice")]
public  		string
  jdPrice { get; set; }


         [XmlElement("outerId")]
public  		string
  outerId { get; set; }


         [XmlElement("barCode")]
public  		string
  barCode { get; set; }


         [XmlElement("categoryId")]
public  		long
  categoryId { get; set; }


         [XmlElement("imgTag")]
public  		int
  imgTag { get; set; }


         [XmlElement("logo")]
public  		string
  logo { get; set; }


         [XmlElement("skuName")]
public  		string
  skuName { get; set; }


         [XmlElement("stockNum")]
public  		long
  stockNum { get; set; }


         [XmlElement("wareTitle")]
public  		string
  wareTitle { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


}
}
