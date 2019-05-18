using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderQueryResult : JdObject{


         [XmlElement("resultCode")]
public  		string
  resultCode { get; set; }


         [XmlElement("errMsg")]
public  		string
  errMsg { get; set; }


         [XmlElement("soNoList")]
public  		List<string>
  soNoList { get; set; }


}
}
