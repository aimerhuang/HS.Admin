using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WareInfo : JdObject{


         [XmlElement("sku")]
public  		long
  sku { get; set; }


         [XmlElement("wareName")]
public  		string
  wareName { get; set; }


         [XmlElement("price")]
public  		string
  price { get; set; }


         [XmlElement("brandName")]
public  		string
  brandName { get; set; }


         [XmlElement("productArea")]
public  		string
  productArea { get; set; }


         [XmlElement("barCode")]
public  		string
  barCode { get; set; }


         [XmlElement("packSpecification")]
public  		string
  packSpecification { get; set; }


         [XmlElement("qrCode")]
public  		string
  qrCode { get; set; }


         [XmlElement("goodRate")]
public  		double
  goodRate { get; set; }


         [XmlElement("hasPromo")]
public  		bool
  hasPromo { get; set; }


         [XmlElement("promoMessage")]
public  		string
  promoMessage { get; set; }


         [XmlElement("deleted")]
public  		int
  deleted { get; set; }


         [XmlElement("state")]
public  		int
  state { get; set; }


}
}
