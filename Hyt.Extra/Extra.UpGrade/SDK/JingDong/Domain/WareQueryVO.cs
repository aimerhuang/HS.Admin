using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WareQueryVO : JdObject{


         [XmlElement("wareId")]
public  		long
  wareId { get; set; }


         [XmlElement("wareStatus")]
public  		int
  wareStatus { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


         [XmlElement("itemNum")]
public  		string
  itemNum { get; set; }


         [XmlElement("transportId")]
public  		int
  transportId { get; set; }


         [XmlElement("onlineTime")]
public  		DateTime
  onlineTime { get; set; }


         [XmlElement("minSupplyPrice")]
public  		string
  minSupplyPrice { get; set; }


         [XmlElement("maxSupplyPrice")]
public  		string
  maxSupplyPrice { get; set; }


         [XmlElement("recommendTpid")]
public  		int
  recommendTpid { get; set; }


}
}
