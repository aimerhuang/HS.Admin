using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class GetFactoryAbutmentOrderInfoResponse : JdResponse{


         [XmlElement("getfactoryabutmentorderinfo_result")]
public  		List<string>
  getfactoryabutmentorderinfoResult { get; set; }


}
}
