using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceExport : JdObject{


         [XmlElement("afsServiceId")]
public  		int
  afsServiceId { get; set; }


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("orderType")]
public  		int
  orderType { get; set; }


         [XmlElement("orderTypeName")]
public  		string
  orderTypeName { get; set; }


         [XmlElement("wareName")]
public  		string
  wareName { get; set; }


         [XmlElement("customerPin")]
public  		string
  customerPin { get; set; }


         [XmlElement("customerName")]
public  		string
  customerName { get; set; }


         [XmlElement("afsServiceProcessResult")]
public  		int
  afsServiceProcessResult { get; set; }


         [XmlElement("afsServiceProcessResultName")]
public  		string
  afsServiceProcessResultName { get; set; }


         [XmlElement("processPin")]
public  		string
  processPin { get; set; }


         [XmlElement("processName")]
public  		string
  processName { get; set; }


         [XmlElement("afsApplyTime")]
public  		DateTime
  afsApplyTime { get; set; }


         [XmlElement("processedDate")]
public  		DateTime
  processedDate { get; set; }


         [XmlElement("afsServiceStatus")]
public  		int
  afsServiceStatus { get; set; }


         [XmlElement("afsServiceStatusName")]
public  		string
  afsServiceStatusName { get; set; }


         [XmlElement("afsServiceStep")]
public  		int
  afsServiceStep { get; set; }


         [XmlElement("approveResonCid1")]
public  		int
  approveResonCid1 { get; set; }


         [XmlElement("approveResonCid2")]
public  		int
  approveResonCid2 { get; set; }


         [XmlElement("wareId")]
public  		int
  wareId { get; set; }


         [XmlElement("approveDate")]
public  		DateTime
  approveDate { get; set; }


         [XmlElement("customerMobilePhone")]
public  		string
  customerMobilePhone { get; set; }


         [XmlElement("customerGrade")]
public  		int
  customerGrade { get; set; }


         [XmlElement("pickwareAddress")]
public  		string
  pickwareAddress { get; set; }


}
}
