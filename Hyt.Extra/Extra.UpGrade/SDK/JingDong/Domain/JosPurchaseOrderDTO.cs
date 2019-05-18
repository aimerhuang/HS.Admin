using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosPurchaseOrderDTO : JdObject{


         [XmlElement("purchaseOrderCode")]
public  		string
  purchaseOrderCode { get; set; }


         [XmlElement("categoryNumber")]
public  		string
  categoryNumber { get; set; }


         [XmlElement("totalNubmer")]
public  		string
  totalNubmer { get; set; }


         [XmlElement("totalAmount")]
public  		string
  totalAmount { get; set; }


         [XmlElement("actualTotalAmount")]
public  		string
  actualTotalAmount { get; set; }


         [XmlElement("purchaseDate")]
public  		DateTime
  purchaseDate { get; set; }


         [XmlElement("supposedArrivingDate")]
public  		DateTime
  supposedArrivingDate { get; set; }


         [XmlElement("buyerContactId")]
public  		string
  buyerContactId { get; set; }


         [XmlElement("buyerContact")]
public  		string
  buyerContact { get; set; }


         [XmlElement("vendorCode")]
public  		string
  vendorCode { get; set; }


         [XmlElement("vendorName")]
public  		string
  vendorName { get; set; }


         [XmlElement("shippingAddress")]
public  		string
  shippingAddress { get; set; }


         [XmlElement("warehouseCode")]
public  		string
  warehouseCode { get; set; }


         [XmlElement("warehouse")]
public  		string
  warehouse { get; set; }


         [XmlElement("orderOwnerCode")]
public  		string
  orderOwnerCode { get; set; }


         [XmlElement("orderOwner")]
public  		string
  orderOwner { get; set; }


         [XmlElement("closingDate")]
public  		DateTime
  closingDate { get; set; }


         [XmlElement("station")]
public  		string
  station { get; set; }


         [XmlElement("payment")]
public  		string
  payment { get; set; }


         [XmlElement("orgCode")]
public  		string
  orgCode { get; set; }


         [XmlElement("orgName")]
public  		string
  orgName { get; set; }


         [XmlElement("comments")]
public  		string
  comments { get; set; }


         [XmlElement("backOrder")]
public  		string
  backOrder { get; set; }


         [XmlElement("tcFlag")]
public  		string
  tcFlag { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("updateTime")]
public  		DateTime
  updateTime { get; set; }


}
}
