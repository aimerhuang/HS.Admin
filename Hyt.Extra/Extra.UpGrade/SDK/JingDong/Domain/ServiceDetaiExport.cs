using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceDetaiExport : JdObject{


         [XmlElement("afsApplyId")]
public  		int
  afsApplyId { get; set; }


         [XmlElement("afsServiceId")]
public  		int
  afsServiceId { get; set; }


         [XmlElement("afsApplyTime")]
public  		DateTime
  afsApplyTime { get; set; }


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("isHasInvoice")]
public  		string
  isHasInvoice { get; set; }


         [XmlElement("isNeedDetectionReport")]
public  		string
  isNeedDetectionReport { get; set; }


         [XmlElement("isHasPackage")]
public  		string
  isHasPackage { get; set; }


         [XmlElement("customerExpect")]
public  		int
  customerExpect { get; set; }


         [XmlElement("questionPic")]
public  		string
  questionPic { get; set; }


         [XmlElement("afsServiceStep")]
public  		int
  afsServiceStep { get; set; }


         [XmlElement("afsServiceStepName")]
public  		string
  afsServiceStepName { get; set; }


         [XmlElement("approveNotes")]
public  		string
  approveNotes { get; set; }


         [XmlElement("questionDesc")]
public  		string
  questionDesc { get; set; }


         [XmlElement("approvedResult")]
public  		int
  approvedResult { get; set; }


         [XmlElement("approvedResultName")]
public  		string
  approvedResultName { get; set; }


         [XmlElement("processResult")]
public  		int
  processResult { get; set; }


         [XmlElement("processResultName")]
public  		string
  processResultName { get; set; }


         [XmlElement("afsServiceStatus")]
public  		int
  afsServiceStatus { get; set; }


         [XmlElement("afsServiceStatusName")]
public  		string
  afsServiceStatusName { get; set; }


         [XmlElement("serviceCustomerInfoExport")]
public  		string
  serviceCustomerInfoExport { get; set; }


         [XmlElement("doorPickwareAddressInfoExport")]
public  		string
  doorPickwareAddressInfoExport { get; set; }


         [XmlElement("receiveWareAddressInfoExport")]
public  		string
  receiveWareAddressInfoExport { get; set; }


         [XmlElement("afterserviceContactsInfoExport")]
public  		string
  afterserviceContactsInfoExport { get; set; }


         [XmlElement("serviceExpressInfoExport")]
public  		string
  serviceExpressInfoExport { get; set; }


         [XmlElement("serviceFinanceDetailInfoExports")]
public  		List<string>
  serviceFinanceDetailInfoExports { get; set; }


         [XmlElement("serviceTrackInfoExports")]
public  		List<string>
  serviceTrackInfoExports { get; set; }


         [XmlElement("serviceDetailInfoExports")]
public  		List<string>
  serviceDetailInfoExports { get; set; }


         [XmlElement("allowOperations")]
public  		List<string>
  allowOperations { get; set; }


         [XmlElement("orderType")]
public  		int
  orderType { get; set; }


         [XmlElement("appointmentInfoExport")]
public  		string
  appointmentInfoExport { get; set; }


         [XmlElement("buId")]
public  		string
  buId { get; set; }


         [XmlElement("approvePin")]
public  		string
  approvePin { get; set; }


         [XmlElement("approveName")]
public  		string
  approveName { get; set; }


         [XmlElement("approvedDate")]
public  		DateTime
  approvedDate { get; set; }


         [XmlElement("processedDate")]
public  		DateTime
  processedDate { get; set; }


         [XmlElement("receiveDate")]
public  		DateTime
  receiveDate { get; set; }


         [XmlElement("afsCategoryId")]
public  		int
  afsCategoryId { get; set; }


         [XmlElement("serviceApplyInfoExport")]
public  		string
  serviceApplyInfoExport { get; set; }


         [XmlElement("companyId")]
public  		int
  companyId { get; set; }


         [XmlElement("pickwareType")]
public  		int
  pickwareType { get; set; }


         [XmlElement("questionTypeCid1")]
public  		int
  questionTypeCid1 { get; set; }


         [XmlElement("questionTypeCid2")]
public  		int
  questionTypeCid2 { get; set; }


         [XmlElement("newOrderId")]
public  		long
  newOrderId { get; set; }


         [XmlElement("updateName")]
public  		string
  updateName { get; set; }


         [XmlElement("updateDate")]
public  		DateTime
  updateDate { get; set; }


         [XmlElement("afsServiceState")]
public  		int
  afsServiceState { get; set; }


         [XmlElement("jdUpgradeSuggestion")]
public  		string
  jdUpgradeSuggestion { get; set; }


}
}
