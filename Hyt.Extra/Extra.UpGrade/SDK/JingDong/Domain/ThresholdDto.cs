using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ThresholdDto : JdObject{


         [XmlElement("min_length")]
public  		int
  minLength { get; set; }


         [XmlElement("max_length")]
public  		int
  maxLength { get; set; }


         [XmlElement("min_width")]
public  		int
  minWidth { get; set; }


         [XmlElement("max_width")]
public  		int
  maxWidth { get; set; }


         [XmlElement("min_height")]
public  		int
  minHeight { get; set; }


         [XmlElement("max_height")]
public  		int
  maxHeight { get; set; }


}
}
