using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EclpOrderQueryOrderResponse : JdResponse{


         [XmlElement("queryorder_result")]
public  		string
  queryorderResult { get; set; }


}
}
