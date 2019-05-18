using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SpacePageInfo : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("available")]
public  		int
  available { get; set; }


         [XmlElement("category_name")]
public  		string
  categoryName { get; set; }


         [XmlElement("parent_id")]
public  		long
  parentId { get; set; }


         [XmlElement("url")]
public  		string
  url { get; set; }


         [XmlElement("type")]
public  		int
  type { get; set; }


}
}
