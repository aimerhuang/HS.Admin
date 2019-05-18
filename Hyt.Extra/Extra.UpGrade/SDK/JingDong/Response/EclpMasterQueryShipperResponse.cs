using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EclpMasterQueryShipperResponse : JdResponse{


         [XmlElement("queryshipper_result")]
public  		List<string>
  queryshipperResult { get; set; }


}
}
