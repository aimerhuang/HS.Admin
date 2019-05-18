using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SpaceInfo : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("detail")]
public  		string
  detail { get; set; }


         [XmlElement("page_id")]
public  		long
  pageId { get; set; }


         [XmlElement("width")]
public  		int
  width { get; set; }


         [XmlElement("height")]
public  		int
  height { get; set; }


         [XmlElement("traffic")]
public  		int
  traffic { get; set; }


         [XmlElement("style")]
public  		int
  style { get; set; }


         [XmlElement("type")]
public  		int
  type { get; set; }


}
}
