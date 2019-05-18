using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EclpRtwQueryRtwResponse : JdResponse{


         [XmlElement("queryrtw_result")]
public  		List<string>
  queryrtwResult { get; set; }


}
}
