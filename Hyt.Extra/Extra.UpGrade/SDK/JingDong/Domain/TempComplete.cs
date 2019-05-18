using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class TempComplete : JdObject{


         [XmlElement("afsServiceId")]
public  		int
  afsServiceId { get; set; }


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("wareId")]
public  		long
  wareId { get; set; }


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


         [XmlElement("afsApplyTime")]
public  		DateTime
  afsApplyTime { get; set; }


         [XmlElement("processDate")]
public  		DateTime
  processDate { get; set; }


         [XmlElement("processPin")]
public  		string
  processPin { get; set; }


         [XmlElement("afsDetailType")]
public  		int
  afsDetailType { get; set; }


         [XmlElement("customerGrade")]
public  		int
  customerGrade { get; set; }


         [XmlElement("customerMobilePhone")]
public  		string
  customerMobilePhone { get; set; }


         [XmlElement("pickwareAddress")]
public  		string
  pickwareAddress { get; set; }


         [XmlElement("approveResonCid1")]
public  		int
  approveResonCid1 { get; set; }


         [XmlElement("approveResonCid2")]
public  		int
  approveResonCid2 { get; set; }


         [XmlElement("afsServiceState")]
public  		int
  afsServiceState { get; set; }


         [XmlElement("afsCategoryId")]
public  		int
  afsCategoryId { get; set; }


}
}
