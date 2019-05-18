using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CampaignQuery : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("dayBudgetStr")]
public  		string
  dayBudgetStr { get; set; }


         [XmlElement("dayBudgetResult")]
public  		double
  dayBudgetResult { get; set; }


         [XmlElement("startTime")]
public  		DateTime
  startTime { get; set; }


         [XmlElement("eneTime")]
public  		DateTime
  eneTime { get; set; }


         [XmlElement("timeRange")]
public  		string
  timeRange { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("spreadType")]
public  		string
  spreadType { get; set; }


         [XmlElement("putType")]
public  		string
  putType { get; set; }


         [XmlElement("businessType")]
public  		string
  businessType { get; set; }


}
}
