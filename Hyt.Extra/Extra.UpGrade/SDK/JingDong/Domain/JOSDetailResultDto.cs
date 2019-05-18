using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JOSDetailResultDto : JdObject{


         [XmlElement("order_id")]
public  		long
  orderId { get; set; }


         [XmlElement("delivery_time")]
public  		DateTime
  deliveryTime { get; set; }


         [XmlElement("record_count")]
public  		int
  recordCount { get; set; }


         [XmlElement("purchase_allocation_detail_list")]
public  		List<string>
  purchaseAllocationDetailList { get; set; }


         [XmlElement("success")]
public  		string
  success { get; set; }


         [XmlElement("result_code")]
public  		string
  resultCode { get; set; }


         [XmlElement("result_message")]
public  		string
  resultMessage { get; set; }


}
}
