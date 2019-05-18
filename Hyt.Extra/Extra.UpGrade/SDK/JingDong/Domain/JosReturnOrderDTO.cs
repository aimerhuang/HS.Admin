using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosReturnOrderDTO : JdObject{


         [XmlElement("vendorCode")]
public  		string
  vendorCode { get; set; }


         [XmlElement("vendorName")]
public  		string
  vendorName { get; set; }


         [XmlElement("returnOrderCode")]
public  		string
  returnOrderCode { get; set; }


         [XmlElement("deliveryNumber")]
public  		string
  deliveryNumber { get; set; }


         [XmlElement("lineNumber")]
public  		int
  lineNumber { get; set; }


         [XmlElement("categoryNumber")]
public  		int
  categoryNumber { get; set; }


         [XmlElement("totalNubmer")]
public  		int
  totalNubmer { get; set; }


         [XmlElement("totalAmount")]
public  		string
  totalAmount { get; set; }


         [XmlElement("actualTotalAmount")]
public  		string
  actualTotalAmount { get; set; }


         [XmlElement("returnDate")]
public  		DateTime
  returnDate { get; set; }


         [XmlElement("shippingAddress")]
public  		string
  shippingAddress { get; set; }


         [XmlElement("freightNum")]
public  		string
  freightNum { get; set; }


         [XmlElement("pakagesNumber")]
public  		int
  pakagesNumber { get; set; }


         [XmlElement("returnOrderStatus")]
public  		int
  returnOrderStatus { get; set; }


         [XmlElement("productType")]
public  		int
  productType { get; set; }


         [XmlElement("remark")]
public  		string
  remark { get; set; }


         [XmlElement("orgCode")]
public  		string
  orgCode { get; set; }


         [XmlElement("orgName")]
public  		string
  orgName { get; set; }


         [XmlElement("warehouseCode")]
public  		string
  warehouseCode { get; set; }


         [XmlElement("warehouse")]
public  		string
  warehouse { get; set; }


         [XmlElement("operatorCode")]
public  		string
  operatorCode { get; set; }


         [XmlElement("operatorName")]
public  		string
  operatorName { get; set; }


         [XmlElement("type")]
public  		int
  type { get; set; }


         [XmlElement("productState")]
public  		int
  productState { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("updateTime")]
public  		DateTime
  updateTime { get; set; }


}
}
