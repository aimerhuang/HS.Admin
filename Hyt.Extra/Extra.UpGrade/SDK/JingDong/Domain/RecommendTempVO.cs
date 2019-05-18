using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class RecommendTempVO : JdObject{


         [XmlElement("id")]
public  		int
  id { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


}
}
