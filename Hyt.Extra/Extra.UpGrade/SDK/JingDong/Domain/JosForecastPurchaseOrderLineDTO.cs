using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosForecastPurchaseOrderLineDTO : JdObject{


         [XmlElement("forecastPurchaseOrderId")]
public  		int
  forecastPurchaseOrderId { get; set; }


         [XmlElement("forecastPurchaseOrderCode")]
public  		string
  forecastPurchaseOrderCode { get; set; }


         [XmlElement("vendorCode")]
public  		string
  vendorCode { get; set; }


         [XmlElement("vendorName")]
public  		string
  vendorName { get; set; }


         [XmlElement("jdSku")]
public  		string
  jdSku { get; set; }


         [XmlElement("productName")]
public  		string
  productName { get; set; }


         [XmlElement("vendorProductId")]
public  		string
  vendorProductId { get; set; }


         [XmlElement("orgCode")]
public  		string
  orgCode { get; set; }


         [XmlElement("orgName")]
public  		string
  orgName { get; set; }


         [XmlElement("warehouseCode")]
public  		string
  warehouseCode { get; set; }


         [XmlElement("warehouseName")]
public  		string
  warehouseName { get; set; }


         [XmlElement("forecastQuantity")]
public  		int
  forecastQuantity { get; set; }


         [XmlElement("forecastSureQuantity")]
public  		int
  forecastSureQuantity { get; set; }


         [XmlElement("forecastSureDifference")]
public  		int
  forecastSureDifference { get; set; }


         [XmlElement("forecastAmount")]
public  		string
  forecastAmount { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("updateTime")]
public  		DateTime
  updateTime { get; set; }


}
}
