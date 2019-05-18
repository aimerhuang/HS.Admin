using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class MarketServiceGetResponse : JdResponse{


         [XmlElement("service_result")]
public  		string
  serviceResult { get; set; }


}
}
