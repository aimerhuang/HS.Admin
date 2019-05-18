using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SafWorkDTO : JdObject{


         [XmlElement("work_id")]
public  		long
  workId { get; set; }


         [XmlElement("order_id")]
public  		long
  orderId { get; set; }


         [XmlElement("status_name")]
public  		string
  statusName { get; set; }


         [XmlElement("create_date")]
public  		DateTime
  createDate { get; set; }


         [XmlElement("content")]
public  		string
  content { get; set; }


}
}
