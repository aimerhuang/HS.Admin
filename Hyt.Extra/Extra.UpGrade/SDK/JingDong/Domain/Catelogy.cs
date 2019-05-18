using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Catelogy : JdObject{


         [XmlElement("fid")]
public  		int
  fid { get; set; }


         [XmlElement("icon")]
public  		string
  icon { get; set; }


         [XmlElement("orderSort")]
public  		int
  orderSort { get; set; }


         [XmlElement("level")]
public  		int
  level { get; set; }


         [XmlElement("description")]
public  		string
  description { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("cid")]
public  		int
  cid { get; set; }


}
}
