using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class ImPopReplystatGetResponse : JdResponse{


         [XmlElement("WaiterDailyStat")]
public  		List<string>
  WaiterDailyStat { get; set; }


}
}
