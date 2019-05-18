using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StockResult : JdObject{


         [XmlElement("resultCode")]
public  		string
  resultCode { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("content")]
public  		List<string>
  content { get; set; }


}
}
