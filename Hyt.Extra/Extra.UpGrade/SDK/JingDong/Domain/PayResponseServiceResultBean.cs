using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PayResponseServiceResultBean : JdObject{


         [XmlElement("payId")]
public  		string
  payId { get; set; }


         [XmlElement("businessId")]
public  		string
  businessId { get; set; }


         [XmlElement("payMethod")]
public  		string
  payMethod { get; set; }


         [XmlElement("bankCode")]
public  		string
  bankCode { get; set; }


         [XmlElement("payStatus")]
public  		string
  payStatus { get; set; }


         [XmlElement("amount")]
public  		string
  amount { get; set; }


         [XmlElement("payTime")]
public  		string
  payTime { get; set; }


         [XmlElement("currency")]
public  		string
  currency { get; set; }


         [XmlElement("resultInfo")]
public  		string
  resultInfo { get; set; }


         [XmlElement("sourceType")]
public  		string
  sourceType { get; set; }


         [XmlElement("success")]
public  		string
  success { get; set; }


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("info")]
public  		string
  info { get; set; }


}
}
