using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryOrderListSkuJos : JdObject{


         [XmlElement("skuId")]
public  		string
  skuId { get; set; }


         [XmlElement("wareId")]
public  		long
  wareId { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


         [XmlElement("imgUrl")]
public  		string
  imgUrl { get; set; }


         [XmlElement("processDays")]
public  		int
  processDays { get; set; }


         [XmlElement("stock")]
public  		string
  stock { get; set; }


         [XmlElement("weight")]
public  		int
  weight { get; set; }


         [XmlElement("quantity")]
public  		string
  quantity { get; set; }


         [XmlElement("costPriceBuy")]
public  		string
  costPriceBuy { get; set; }


         [XmlElement("jdPriceBuy")]
public  		string
  jdPriceBuy { get; set; }


         [XmlElement("rfId")]
public  		string
  rfId { get; set; }


         [XmlElement("promDisBuy")]
public  		string
  promDisBuy { get; set; }


         [XmlElement("couponDisBuy")]
public  		string
  couponDisBuy { get; set; }


         [XmlElement("inPrice")]
public  		string
  inPrice { get; set; }


         [XmlElement("customs")]
public  		string
  customs { get; set; }


         [XmlElement("promType")]
public  		string
  promType { get; set; }


         [XmlElement("promUrl")]
public  		string
  promUrl { get; set; }


         [XmlElement("promName")]
public  		string
  promName { get; set; }


         [XmlElement("promCategory")]
public  		string
  promCategory { get; set; }


         [XmlElement("carrierCode")]
public  		int
  carrierCode { get; set; }


         [XmlElement("carrierName")]
public  		string
  carrierName { get; set; }


         [XmlElement("arrivedDays")]
public  		string
  arrivedDays { get; set; }


         [XmlElement("transportId")]
public  		long
  transportId { get; set; }


         [XmlElement("shipCostBuy")]
public  		string
  shipCostBuy { get; set; }


         [XmlElement("shipDisBuy")]
public  		string
  shipDisBuy { get; set; }


}
}
