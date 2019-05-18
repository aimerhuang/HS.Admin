using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class RtsResult : JdObject{


         [XmlElement("eclpRtsNo")]
public  		string
  eclpRtsNo { get; set; }


         [XmlElement("isvRtsNum")]
public  		string
  isvRtsNum { get; set; }


         [XmlElement("deptNo")]
public  		string
  deptNo { get; set; }


         [XmlElement("deliveryMode")]
public  		string
  deliveryMode { get; set; }


         [XmlElement("supplierNo")]
public  		string
  supplierNo { get; set; }


         [XmlElement("rtsOrderStatus")]
public  		string
  rtsOrderStatus { get; set; }


         [XmlElement("operatorTime")]
public  		string
  operatorTime { get; set; }


         [XmlElement("operatorUser")]
public  		string
  operatorUser { get; set; }


         [XmlElement("source")]
public  		string
  source { get; set; }


         [XmlElement("remark")]
public  		string
  remark { get; set; }


         [XmlElement("receiver")]
public  		string
  receiver { get; set; }


         [XmlElement("receiverPhone")]
public  		string
  receiverPhone { get; set; }


         [XmlElement("email")]
public  		string
  email { get; set; }


         [XmlElement("province")]
public  		string
  province { get; set; }


         [XmlElement("city")]
public  		string
  city { get; set; }


         [XmlElement("county")]
public  		string
  county { get; set; }


         [XmlElement("town")]
public  		string
  town { get; set; }


         [XmlElement("warehouseNo")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("rtsDetailList")]
public  		List<string>
  rtsDetailList { get; set; }


         [XmlElement("resultCode")]
public  		string
  resultCode { get; set; }


         [XmlElement("msg")]
public  		string
  msg { get; set; }


}
}
