using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PropGroupDto : JdObject{


         [XmlElement("id")]
public  		int
  id { get; set; }


         [XmlElement("order_sort")]
public  		int
  orderSort { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("props")]
public  		List<string>
  props { get; set; }


}
}
