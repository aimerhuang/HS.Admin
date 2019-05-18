using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AfsFinanceOut : JdObject{


         [XmlElement("userName")]
public  		string
  userName { get; set; }


         [XmlElement("userId")]
public  		string
  userId { get; set; }


         [XmlElement("serviceId")]
public  		int
  serviceId { get; set; }


         [XmlElement("idFinance")]
public  		int
  idFinance { get; set; }


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("reason")]
public  		string
  reason { get; set; }


         [XmlElement("bilv")]
public  		string
  bilv { get; set; }


         [XmlElement("price")]
public  		string
  price { get; set; }


         [XmlElement("margReason")]
public  		string
  margReason { get; set; }


         [XmlElement("refundment")]
public  		string
  refundment { get; set; }


         [XmlElement("carriage")]
public  		string
  carriage { get; set; }


         [XmlElement("rebate")]
public  		string
  rebate { get; set; }


         [XmlElement("type")]
public  		string
  type { get; set; }


         [XmlElement("bank")]
public  		string
  bank { get; set; }


         [XmlElement("wareId")]
public  		int
  wareId { get; set; }


         [XmlElement("wareName")]
public  		string
  wareName { get; set; }


         [XmlElement("memberPhone")]
public  		string
  memberPhone { get; set; }


         [XmlElement("opPin")]
public  		string
  opPin { get; set; }


         [XmlElement("opName")]
public  		string
  opName { get; set; }


         [XmlElement("opTime")]
public  		string
  opTime { get; set; }


         [XmlElement("account")]
public  		string
  account { get; set; }


         [XmlElement("codeAccount")]
public  		string
  codeAccount { get; set; }


         [XmlElement("notes")]
public  		string
  notes { get; set; }


}
}
