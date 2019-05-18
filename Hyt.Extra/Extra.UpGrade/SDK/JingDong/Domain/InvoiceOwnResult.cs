using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class InvoiceOwnResult : JdObject{


         [XmlElement("success")]
public  		string
  success { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


}
}
