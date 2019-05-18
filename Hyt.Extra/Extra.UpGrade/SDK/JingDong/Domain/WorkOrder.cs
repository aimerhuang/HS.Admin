using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WorkOrder : JdObject{


         [XmlElement("work_id")]
public  		long
  workId { get; set; }


         [XmlElement("order_id")]
public  		long
  orderId { get; set; }


         [XmlElement("work_type")]
public  		long
  workType { get; set; }


         [XmlElement("child_work_id")]
public  		long
  childWorkId { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


         [XmlElement("content")]
public  		string
  content { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


         [XmlElement("use_flag")]
public  		int
  useFlag { get; set; }


}
}
