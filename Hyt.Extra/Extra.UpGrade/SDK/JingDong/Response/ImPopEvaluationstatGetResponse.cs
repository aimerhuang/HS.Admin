using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class ImPopEvaluationstatGetResponse : JdResponse{


         [XmlElement("WaiterDailyEvaStat")]
public  		List<string>
  WaiterDailyEvaStat { get; set; }


}
}
