using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class GetFactoryAbutmentCancelInfoResponse : JdResponse{


         [XmlElement("getfactoryabutmentcancelinfo_result")]
public  		List<string>
  getfactoryabutmentcancelinfoResult { get; set; }


}
}
