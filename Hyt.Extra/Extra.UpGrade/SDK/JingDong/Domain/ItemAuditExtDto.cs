using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ItemAuditExtDto : JdObject{


         [XmlElement("erp_code")]
public  		string
  erpCode { get; set; }


         [XmlElement("opinion")]
public  		string
  opinion { get; set; }


         [XmlElement("operate_time")]
public  		DateTime
  operateTime { get; set; }


         [XmlElement("state")]
public  		int
  state { get; set; }


         [XmlElement("task_id")]
public  		string
  taskId { get; set; }


}
}
