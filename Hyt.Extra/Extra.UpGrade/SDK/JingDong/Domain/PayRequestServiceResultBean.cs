using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PayRequestServiceResultBean : JdObject{


         [XmlElement("payId")]
public  		string
  payId { get; set; }


         [XmlElement("businessId")]
public  		string
  businessId { get; set; }


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
