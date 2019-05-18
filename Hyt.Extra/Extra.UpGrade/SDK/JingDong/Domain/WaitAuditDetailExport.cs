using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WaitAuditDetailExport : JdObject{


         [XmlElement("serviceIdList")]
public  		List<string>
  serviceIdList { get; set; }


         [XmlElement("customerExpect")]
public  		int
  customerExpect { get; set; }


         [XmlElement("afsApplyTime")]
public  		DateTime
  afsApplyTime { get; set; }


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("questionPic")]
public  		string
  questionPic { get; set; }


         [XmlElement("questionDesc")]
public  		string
  questionDesc { get; set; }


         [XmlElement("questionTypeCid1")]
public  		int
  questionTypeCid1 { get; set; }


         [XmlElement("questionTypeCid2")]
public  		int
  questionTypeCid2 { get; set; }


         [XmlElement("orderType")]
public  		int
  orderType { get; set; }


         [XmlElement("serviceCustomerInfoExport")]
public  		string
  serviceCustomerInfoExport { get; set; }


         [XmlElement("applyDetailInfoExport")]
public  		List<string>
  applyDetailInfoExport { get; set; }


         [XmlElement("doorPickwareAddressInfoExport")]
public  		string
  doorPickwareAddressInfoExport { get; set; }


         [XmlElement("receiveWareAddressInfoExport")]
public  		string
  receiveWareAddressInfoExport { get; set; }


         [XmlElement("appointmentInfoExport")]
public  		string
  appointmentInfoExport { get; set; }


         [XmlElement("afsCategoryId")]
public  		int
  afsCategoryId { get; set; }


         [XmlElement("expectPickwareType")]
public  		int
  expectPickwareType { get; set; }


         [XmlElement("invoiceCode")]
public  		string
  invoiceCode { get; set; }


         [XmlElement("jdUpgradeSuggestion")]
public  		string
  jdUpgradeSuggestion { get; set; }


}
}
