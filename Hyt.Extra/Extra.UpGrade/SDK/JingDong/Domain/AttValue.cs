using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AttValue : JdObject{


         [XmlElement("aid")]
public  		string
  aid { get; set; }


         [XmlElement("vid")]
public  		string
  vid { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("index_id")]
public  		string
  indexId { get; set; }


         [XmlElement("features")]
public  		string
  features { get; set; }


}
}
