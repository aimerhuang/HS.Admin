using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AfsServiceOut : JdObject{


         [XmlElement("afsServiceId")]
public  		int
  afsServiceId { get; set; }


         [XmlElement("afsCategoryId")]
public  		int
  afsCategoryId { get; set; }


         [XmlElement("afsApplyId")]
public  		int
  afsApplyId { get; set; }


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("orderRemark")]
public  		string
  orderRemark { get; set; }


         [XmlElement("wareId")]
public  		int
  wareId { get; set; }


         [XmlElement("wareName")]
public  		string
  wareName { get; set; }


         [XmlElement("pickwareProvince")]
public  		int
  pickwareProvince { get; set; }


         [XmlElement("pickwareCity")]
public  		int
  pickwareCity { get; set; }


         [XmlElement("pickwareCounty")]
public  		int
  pickwareCounty { get; set; }


         [XmlElement("pickwareVillage")]
public  		int
  pickwareVillage { get; set; }


         [XmlElement("pickwareAddress")]
public  		string
  pickwareAddress { get; set; }


         [XmlElement("returnwareProvince")]
public  		int
  returnwareProvince { get; set; }


         [XmlElement("returnwareCity")]
public  		int
  returnwareCity { get; set; }


         [XmlElement("returnwareCounty")]
public  		int
  returnwareCounty { get; set; }


         [XmlElement("returnwareVillage")]
public  		int
  returnwareVillage { get; set; }


         [XmlElement("returnwareAddress")]
public  		string
  returnwareAddress { get; set; }


         [XmlElement("customerExpect")]
public  		int
  customerExpect { get; set; }


         [XmlElement("questionDesc")]
public  		string
  questionDesc { get; set; }


         [XmlElement("customerName")]
public  		string
  customerName { get; set; }


         [XmlElement("customerMobilePhone")]
public  		string
  customerMobilePhone { get; set; }


         [XmlElement("customerEmail")]
public  		string
  customerEmail { get; set; }


         [XmlElement("approveName")]
public  		string
  approveName { get; set; }


         [XmlElement("afsApplyTime")]
public  		DateTime
  afsApplyTime { get; set; }


         [XmlElement("approvedDate")]
public  		DateTime
  approvedDate { get; set; }


         [XmlElement("processedDate")]
public  		DateTime
  processedDate { get; set; }


         [XmlElement("receiveDate")]
public  		DateTime
  receiveDate { get; set; }


         [XmlElement("createName")]
public  		string
  createName { get; set; }


         [XmlElement("createDate")]
public  		DateTime
  createDate { get; set; }


}
}
