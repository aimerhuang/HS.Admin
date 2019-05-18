using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ReturnInfo : JdObject{


         [XmlElement("return_id")]
public  		string
  returnId { get; set; }


         [XmlElement("vender_id")]
public  		string
  venderId { get; set; }


         [XmlElement("sendType")]
public  		string
  sendType { get; set; }


         [XmlElement("receive_state")]
public  		string
  receiveState { get; set; }


         [XmlElement("linkman")]
public  		string
  linkman { get; set; }


         [XmlElement("phone")]
public  		string
  phone { get; set; }


         [XmlElement("return_address")]
public  		string
  returnAddress { get; set; }


         [XmlElement("consignee")]
public  		string
  consignee { get; set; }


         [XmlElement("consignor")]
public  		string
  consignor { get; set; }


         [XmlElement("send_time")]
public  		string
  sendTime { get; set; }


         [XmlElement("receive_time")]
public  		string
  receiveTime { get; set; }


         [XmlElement("modifid_time")]
public  		string
  modifidTime { get; set; }


         [XmlElement("return_item_list")]
public  		List<string>
  returnItemList { get; set; }


}
}
