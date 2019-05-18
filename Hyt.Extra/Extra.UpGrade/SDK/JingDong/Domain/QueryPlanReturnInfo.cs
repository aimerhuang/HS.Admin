using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryPlanReturnInfo : JdObject{


         [XmlElement("total_number")]
public  		int
  totalNumber { get; set; }


         [XmlElement("plan_list")]
public  		List<string>
  planList { get; set; }


}
}
