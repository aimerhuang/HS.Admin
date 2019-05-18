using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class BrandVo : JdObject{


         [XmlElement("id")]
public  		int
  id { get; set; }


         [XmlElement("brandId")]
public  		int
  brandId { get; set; }


         [XmlElement("brandName")]
public  		string
  brandName { get; set; }


         [XmlElement("cnName")]
public  		string
  cnName { get; set; }


         [XmlElement("enName")]
public  		string
  enName { get; set; }


         [XmlElement("logo")]
public  		string
  logo { get; set; }


         [XmlElement("firstChar")]
public  		string
  firstChar { get; set; }


         [XmlElement("endTime")]
public  		DateTime
  endTime { get; set; }


         [XmlElement("isForever")]
public  		int
  isForever { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


}
}
