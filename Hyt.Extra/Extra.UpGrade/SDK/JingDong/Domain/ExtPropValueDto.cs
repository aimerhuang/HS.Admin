using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ExtPropValueDto : JdObject{


         [XmlElement("value_id")]
public  		int
  valueId { get; set; }


         [XmlElement("att_id")]
public  		int
  attId { get; set; }


         [XmlElement("value_name")]
public  		string
  valueName { get; set; }


         [XmlElement("type")]
public  		int
  type { get; set; }


         [XmlElement("yn")]
public  		int
  yn { get; set; }


         [XmlElement("brand_prx")]
public  		string
  brandPrx { get; set; }


         [XmlElement("sort")]
public  		int
  sort { get; set; }


         [XmlElement("grade_avg")]
public  		int
  gradeAvg { get; set; }


         [XmlElement("remarks")]
public  		string
  remarks { get; set; }


         [XmlElement("is_required")]
public  		int
  isRequired { get; set; }


         [XmlElement("alias")]
public  		string
  alias { get; set; }


}
}
