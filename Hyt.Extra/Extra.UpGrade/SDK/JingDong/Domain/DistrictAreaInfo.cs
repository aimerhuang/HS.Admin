using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class DistrictAreaInfo : JdObject{


         [XmlElement("id")]
public  		string
  id { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("type")]
public  		string
  type { get; set; }


}
}
