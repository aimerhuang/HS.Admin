using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosPurchaseOrderLineDTO : JdObject{


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
public  		int
  quantity { get; set; }


         [XmlElement("salePrice")]
public  		double
  salePrice { get; set; }


         [XmlElement("discountRate")]
public  		double
  discountRate { get; set; }


         [XmlElement("productCode")]
public  		string
  productCode { get; set; }


         [XmlElement("listPrice")]
public  		double
  listPrice { get; set; }


         [XmlElement("backOrderProcessing")]
public  		string
  backOrderProcessing { get; set; }


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
