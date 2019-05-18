using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WaitAuditApplyExport : JdObject{


         [XmlElement("serviceIdList")]
public  		List<string>
  serviceIdList { get; set; }


         [XmlElement("afsApplyId")]
public  		int
  afsApplyId { get; set; }


         [XmlElement("customerPin")]
public  		string
  customerPin { get; set; }


         [XmlElement("customerName")]
public  		string
  customerName { get; set; }


         [XmlElement("customerExpect")]
public  		int
  customerExpect { get; set; }


         [XmlElement("customerExpectName")]
public  		string
  customerExpectName { get; set; }


         [XmlElement("afsServiceStatusName")]
public  		string
  afsServiceStatusName { get; set; }


         [XmlElement("afsServiceStatus")]
public  		int
  afsServiceStatus { get; set; }


         [XmlElement("afsApplyTime")]
public  		DateTime
  afsApplyTime { get; set; }


         [XmlElement("auditOvertime")]
public  		DateTime
  auditOvertime { get; set; }


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("wareId")]
public  		long
  wareId { get; set; }


         [XmlElement("wareName")]
public  		string
  wareName { get; set; }


         [XmlElement("ifTimeoutSoon")]
public  		bool
  ifTimeoutSoon { get; set; }


         [XmlElement("actualPayPrice")]
public  		string
  actualPayPrice { get; set; }


         [XmlElement("customerMobilePhone")]
public  		string
  customerMobilePhone { get; set; }


         [XmlElement("customerGrade")]
public  		int
  customerGrade { get; set; }


         [XmlElement("pickwareAddress")]
public  		string
  pickwareAddress { get; set; }


         [XmlElement("orderType")]
public  		int
  orderType { get; set; }


}
}
