using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderTaskResult : JdObject{


         [XmlElement("taskId")]
public  		long
  taskId { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("msg")]
public  		string
  msg { get; set; }


         [XmlElement("orderId")]
public  		string
  orderId { get; set; }


         [XmlElement("actualPayMoney")]
public  		string
  actualPayMoney { get; set; }


         [XmlElement("goodsDetails")]
public  		List<string>
  goodsDetails { get; set; }


}
}
