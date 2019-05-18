using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AddressInfoExport : JdObject{


         [XmlElement("province")]
public  		string
  province { get; set; }


         [XmlElement("city")]
public  		string
  city { get; set; }


         [XmlElement("county")]
public  		string
  county { get; set; }


         [XmlElement("village")]
public  		string
  village { get; set; }


         [XmlElement("detailAddress")]
public  		string
  detailAddress { get; set; }


}
}
