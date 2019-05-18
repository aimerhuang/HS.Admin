using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ImgzoneZoneInfo : JdObject{


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("used_size")]
public  		int
  usedSize { get; set; }


         [XmlElement("total_size")]
public  		int
  totalSize { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


}
}
