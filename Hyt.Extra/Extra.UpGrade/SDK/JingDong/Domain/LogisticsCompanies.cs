using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class LogisticsCompanies : JdObject{


         [XmlElement("logistics_list")]
public  		List<string>
  logisticsList { get; set; }


         [XmlElement("vender_id")]
public  		string
  venderId { get; set; }


}
}
