using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EclpOrderQueryOrderListResponse : JdResponse{


         [XmlElement("queryordervmi_result")]
public  		List<string>
  queryordervmiResult { get; set; }


}
}
