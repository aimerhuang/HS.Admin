using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosLargeSparePartInventoryDTO : JdObject{


         [XmlElement("orgId")]
public  		string
  orgId { get; set; }


         [XmlElement("orgName")]
public  		string
  orgName { get; set; }


         [XmlElement("distribCenterCode")]
public  		string
  distribCenterCode { get; set; }


         [XmlElement("distribCenterName")]
public  		string
  distribCenterName { get; set; }


         [XmlElement("warehouseCode")]
public  		string
  warehouseCode { get; set; }


         [XmlElement("warehouseName")]
public  		string
  warehouseName { get; set; }


         [XmlElement("jdSku")]
public  		string
  jdSku { get; set; }


         [XmlElement("productName")]
public  		string
  productName { get; set; }


         [XmlElement("quantity")]
public  		int
  quantity { get; set; }


         [XmlElement("partCode")]
public  		string
  partCode { get; set; }


         [XmlElement("brandId")]
public  		string
  brandId { get; set; }


         [XmlElement("brandName")]
public  		string
  brandName { get; set; }


         [XmlElement("salerErpPin")]
public  		string
  salerErpPin { get; set; }


         [XmlElement("inTime")]
public  		DateTime
  inTime { get; set; }


}
}
