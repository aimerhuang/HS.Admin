using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VideoBigFieldInfo : JdObject{


         [XmlElement("comments")]
public  		string
  comments { get; set; }


         [XmlElement("image")]
public  		string
  image { get; set; }


         [XmlElement("contentDesc")]
public  		string
  contentDesc { get; set; }


         [XmlElement("editerDesc")]
public  		string
  editerDesc { get; set; }


         [XmlElement("catalogue")]
public  		string
  catalogue { get; set; }


         [XmlElement("box_Contents")]
public  		string
  boxContents { get; set; }


         [XmlElement("material_Description")]
public  		string
  materialDescription { get; set; }


         [XmlElement("manual")]
public  		string
  manual { get; set; }


         [XmlElement("productFeatures")]
public  		string
  productFeatures { get; set; }


}
}
