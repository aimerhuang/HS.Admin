using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ItemAttrDetailDto : JdObject{


         [XmlElement("ware_id")]
public  		string
  wareId { get; set; }


         [XmlElement("ware_name")]
public  		string
  wareName { get; set; }


         [XmlElement("color_name")]
public  		string
  colorName { get; set; }


         [XmlElement("color_sort")]
public  		string
  colorSort { get; set; }


         [XmlElement("size_name")]
public  		string
  sizeName { get; set; }


         [XmlElement("size_sort")]
public  		string
  sizeSort { get; set; }


}
}
