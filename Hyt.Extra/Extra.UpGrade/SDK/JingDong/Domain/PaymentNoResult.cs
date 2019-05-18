using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PaymentNoResult : JdObject{


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("errMsg")]
public  		string
  errMsg { get; set; }


         [XmlElement("paymentNo")]
public  		string
  paymentNo { get; set; }


         [XmlElement("parentPaymentNo")]
public  		string
  parentPaymentNo { get; set; }


}
}
