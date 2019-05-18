using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CategoryBasicInfo : JdObject{


         [XmlElement("city_id")]
public  		string
  cityId { get; set; }


         [XmlElement("city_name")]
public  		string
  cityName { get; set; }


         [XmlElement("city_letter")]
public  		string
  cityLetter { get; set; }


         [XmlElement("sort_order")]
public  		string
  sortOrder { get; set; }


         [XmlElement("feature")]
public  		string
  feature { get; set; }


}
}
