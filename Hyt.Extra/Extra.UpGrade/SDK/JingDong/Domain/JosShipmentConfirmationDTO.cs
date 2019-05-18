using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosShipmentConfirmationDTO : JdObject{


         [XmlElement("purchaseOrderCode")]
public  		string
  purchaseOrderCode { get; set; }


         [XmlElement("orderCode")]
public  		string
  orderCode { get; set; }


         [XmlElement("vendorCode")]
public  		string
  vendorCode { get; set; }


         [XmlElement("vendorName")]
public  		string
  vendorName { get; set; }


         [XmlElement("receivingTotalCategoryQty")]
public  		int
  receivingTotalCategoryQty { get; set; }


         [XmlElement("receivingTotalQty")]
public  		int
  receivingTotalQty { get; set; }


         [XmlElement("receivingAmount")]
public  		double
  receivingAmount { get; set; }


         [XmlElement("actualReceivingAmount")]
public  		double
  actualReceivingAmount { get; set; }


         [XmlElement("dispatchDate")]
public  		DateTime
  dispatchDate { get; set; }


         [XmlElement("receivingDate")]
public  		DateTime
  receivingDate { get; set; }


         [XmlElement("firstReceivingDate")]
public  		DateTime
  firstReceivingDate { get; set; }


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
