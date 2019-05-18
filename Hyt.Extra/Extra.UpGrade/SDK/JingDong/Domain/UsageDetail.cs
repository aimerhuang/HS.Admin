using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class UsageDetail : JdObject{


         [XmlElement("sub_id")]
public  		int
  subId { get; set; }


         [XmlElement("pin")]
public  		string
  pin { get; set; }


         [XmlElement("purchase_amount")]
public  		int
  purchaseAmount { get; set; }


         [XmlElement("application_amount")]
public  		int
  applicationAmount { get; set; }


         [XmlElement("available_amount")]
public  		int
  availableAmount { get; set; }


         [XmlElement("service_item_code")]
public  		string
  serviceItemCode { get; set; }


         [XmlElement("start_date")]
public  		DateTime
  startDate { get; set; }


         [XmlElement("end_date")]
public  		DateTime
  endDate { get; set; }


         [XmlElement("price")]
public  		string
  price { get; set; }


}
}
