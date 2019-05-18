using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderExportResponse : JdObject{


         [XmlElement("process_code")]
public  		string
  processCode { get; set; }


         [XmlElement("process_status")]
public  		string
  processStatus { get; set; }


         [XmlElement("error_message")]
public  		string
  errorMessage { get; set; }


         [XmlElement("order_id")]
public  		string
  orderId { get; set; }


         [XmlElement("vender_id")]
public  		string
  venderId { get; set; }


         [XmlElement("order_update_time")]
public  		string
  orderUpdateTime { get; set; }


}
}
