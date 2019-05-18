using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WarePropimg : JdObject{


         [XmlElement("ware_id")]
public  		long
  wareId { get; set; }


         [XmlElement("img_id")]
public  		int
  imgId { get; set; }


         [XmlElement("color_id")]
public  		string
  colorId { get; set; }


         [XmlElement("img_url")]
public  		string
  imgUrl { get; set; }


         [XmlElement("isMain")]
public  		string
  isMain { get; set; }


         [XmlElement("created")]
public  		string
  created { get; set; }


}
}
