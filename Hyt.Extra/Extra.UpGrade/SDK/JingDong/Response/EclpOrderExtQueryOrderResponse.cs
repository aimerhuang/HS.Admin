using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EclpOrderExtQueryOrderResponse : JdResponse{


         [XmlElement("queryorder_result")]
public  		List<string>
  queryorderResult { get; set; }


}
}
