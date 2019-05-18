using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VatIncoiceInfo : JdObject{


         [XmlElement("vatNo")]
public  		string
  vatNo { get; set; }


         [XmlElement("addressRegIstered")]
public  		string
  addressRegIstered { get; set; }


         [XmlElement("phoneRegIstered")]
public  		string
  phoneRegIstered { get; set; }


         [XmlElement("depositBank")]
public  		string
  depositBank { get; set; }


         [XmlElement("bankAccount")]
public  		string
  bankAccount { get; set; }


}
}
