using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryMap : JdObject{


         [XmlElement("id")]
public  		string
  id { get; set; }


         [XmlElement("buyer_id")]
public  		string
  buyerId { get; set; }


         [XmlElement("buyer_name")]
public  		string
  buyerName { get; set; }


         [XmlElement("check_time")]
public  		string
  checkTime { get; set; }


         [XmlElement("apply_time")]
public  		string
  applyTime { get; set; }


         [XmlElement("apply_refund_sum")]
public  		string
  applyRefundSum { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("check_username")]
public  		string
  checkUsername { get; set; }


         [XmlElement("order_id")]
public  		string
  orderId { get; set; }


}
}
