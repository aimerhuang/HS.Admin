using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Image : JdObject{


         [XmlElement("colorId")]
public  		string
  colorId { get; set; }


         [XmlElement("imgId")]
public  		long
  imgId { get; set; }


         [XmlElement("imgIndex")]
public  		int
  imgIndex { get; set; }


         [XmlElement("imgUrl")]
public  		string
  imgUrl { get; set; }


         [XmlElement("imgZoneId")]
public  		string
  imgZoneId { get; set; }


}
}
