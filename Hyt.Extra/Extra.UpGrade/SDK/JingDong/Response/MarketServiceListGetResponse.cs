using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class MarketServiceListGetResponse : JdResponse{


         [XmlElement("services_result")]
public  		string
  servicesResult { get; set; }


}
}
