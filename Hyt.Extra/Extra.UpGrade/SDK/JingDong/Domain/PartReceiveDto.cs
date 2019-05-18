using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PartReceiveDto : JdObject{


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


         [XmlElement("wareId")]
public  		int
  wareId { get; set; }


         [XmlElement("customerName")]
public  		string
  customerName { get; set; }


         [XmlElement("customerExpect")]
public  		int
  customerExpect { get; set; }


         [XmlElement("customerExpectName")]
public  		string
  customerExpectName { get; set; }


         [XmlElement("afsServiceStatus")]
public  		int
  afsServiceStatus { get; set; }


         [XmlElement("afsServiceStatusName")]
public  		string
  afsServiceStatusName { get; set; }


         [XmlElement("afsApplyTime")]
public  		DateTime
  afsApplyTime { get; set; }


         [XmlElement("afsApprovedTime")]
public  		DateTime
  afsApprovedTime { get; set; }


         [XmlElement("customerPin")]
public  		string
  customerPin { get; set; }


         [XmlElement("afsCategoryId")]
public  		int
  afsCategoryId { get; set; }


         [XmlElement("afsDetailType")]
public  		int
  afsDetailType { get; set; }


         [XmlElement("approveResonCid2")]
public  		int
  approveResonCid2 { get; set; }


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
