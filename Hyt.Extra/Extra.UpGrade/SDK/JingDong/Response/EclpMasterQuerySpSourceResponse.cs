using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EclpMasterQuerySpSourceResponse : JdResponse{


         [XmlElement("queryspsource_result")]
public  		List<string>
  queryspsourceResult { get; set; }


}
}
