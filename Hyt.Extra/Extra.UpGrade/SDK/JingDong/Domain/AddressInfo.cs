using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AddressInfo : JdObject{


         [XmlElement("receiver")]
public  		string
  receiver { get; set; }


         [XmlElement("mobile")]
public  		string
  mobile { get; set; }


         [XmlElement("province")]
public  		string
  province { get; set; }


         [XmlElement("city")]
public  		string
  city { get; set; }


         [XmlElement("area")]
public  		string
  area { get; set; }


         [XmlElement("detail")]
public  		string
  detail { get; set; }


}
}
