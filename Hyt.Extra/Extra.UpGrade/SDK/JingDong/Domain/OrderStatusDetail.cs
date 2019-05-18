using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderStatusDetail : JdObject{


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("status_name")]
public  		string
  statusName { get; set; }


         [XmlElement("complete_time")]
public  		string
  completeTime { get; set; }


}
}
