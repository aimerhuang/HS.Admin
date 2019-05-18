using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WorkOrderDetail : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("work_id")]
public  		long
  workId { get; set; }


         [XmlElement("account")]
public  		string
  account { get; set; }


         [XmlElement("account_type")]
public  		int
  accountType { get; set; }


         [XmlElement("work_type")]
public  		long
  workType { get; set; }


         [XmlElement("work2_type")]
public  		long
  work2Type { get; set; }


         [XmlElement("reply_info")]
public  		string
  replyInfo { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


         [XmlElement("wait_reply_time")]
public  		DateTime
  waitReplyTime { get; set; }


}
}
