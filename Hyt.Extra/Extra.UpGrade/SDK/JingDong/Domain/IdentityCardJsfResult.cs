using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class IdentityCardJsfResult : JdObject{


         [XmlElement("success")]
public  		string
  success { get; set; }


         [XmlElement("resultCode")]
public  		string
  resultCode { get; set; }


         [XmlElement("errorMsg")]
public  		string
  errorMsg { get; set; }


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("identityCardNum")]
public  		string
  identityCardNum { get; set; }


}
}
