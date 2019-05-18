using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class KeywordGroup : JdObject{


         [XmlElement("wgroup_id")]
public  		long
  wgroupId { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("search_num")]
public  		long
  searchNum { get; set; }


         [XmlElement("base_price")]
public  		string
  basePrice { get; set; }


         [XmlElement("avg_price")]
public  		string
  avgPrice { get; set; }


         [XmlElement("week_ctr")]
public  		string
  weekCtr { get; set; }


}
}
