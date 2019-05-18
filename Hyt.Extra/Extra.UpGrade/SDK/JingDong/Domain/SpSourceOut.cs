using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SpSourceOut : JdObject{


         [XmlElement("spSourceNo")]
public  		string
  spSourceNo { get; set; }


         [XmlElement("spSourceName")]
public  		string
  spSourceName { get; set; }


         [XmlElement("website")]
public  		string
  website { get; set; }


         [XmlElement("reserve1")]
public  		string
  reserve1 { get; set; }


         [XmlElement("reserve2")]
public  		string
  reserve2 { get; set; }


         [XmlElement("reserve3")]
public  		string
  reserve3 { get; set; }


         [XmlElement("reserve4")]
public  		string
  reserve4 { get; set; }


         [XmlElement("reserve5")]
public  		string
  reserve5 { get; set; }


}
}
