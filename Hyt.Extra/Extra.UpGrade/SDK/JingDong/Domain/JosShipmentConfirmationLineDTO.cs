using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosShipmentConfirmationLineDTO : JdObject{


         [XmlElement("currentRecordCount")]
public  		string
  currentRecordCount { get; set; }


         [XmlElement("vendorSku")]
public  		string
  vendorSku { get; set; }


         [XmlElement("buyerProductId")]
public  		string
  buyerProductId { get; set; }


         [XmlElement("productName")]
public  		string
  productName { get; set; }


         [XmlElement("quantity")]
public  		string
  quantity { get; set; }


         [XmlElement("receivingQty")]
public  		string
  receivingQty { get; set; }


         [XmlElement("salePrice")]
public  		string
  salePrice { get; set; }


         [XmlElement("packageNum")]
public  		string
  packageNum { get; set; }


         [XmlElement("packFirmNumber")]
public  		string
  packFirmNumber { get; set; }


         [XmlElement("firmRoyalty")]
public  		string
  firmRoyalty { get; set; }


         [XmlElement("diffDescription")]
public  		string
  diffDescription { get; set; }


         [XmlElement("receivingDate")]
public  		DateTime
  receivingDate { get; set; }


         [XmlElement("comments")]
public  		string
  comments { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("updateTime")]
public  		DateTime
  updateTime { get; set; }


}
}
