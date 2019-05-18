using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AnnouncementVO : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("type")]
public  		int
  type { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


         [XmlElement("content")]
public  		string
  content { get; set; }


         [XmlElement("content_type")]
public  		string
  contentType { get; set; }


         [XmlElement("publish_time")]
public  		string
  publishTime { get; set; }


}
}
