using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EclpOrderQueryOrderListByStatusResponse : JdResponse{


         [XmlElement("orderQueryResult")]
public  		string
  orderQueryResult { get; set; }


}
}
