using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AfsServiceHistory : JdObject{


         [XmlElement("afsServiceId")]
public  		int
  afsServiceId { get; set; }


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("customerName")]
public  		string
  customerName { get; set; }


         [XmlElement("afsApplyTime")]
public  		DateTime
  afsApplyTime { get; set; }


         [XmlElement("approvedDate")]
public  		DateTime
  approvedDate { get; set; }


         [XmlElement("processedDate")]
public  		DateTime
  processedDate { get; set; }


}
}
