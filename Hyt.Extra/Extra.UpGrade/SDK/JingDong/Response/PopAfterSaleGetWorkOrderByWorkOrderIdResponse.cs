using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class PopAfterSaleGetWorkOrderByWorkOrderIdResponse : JdResponse{


         [XmlElement("query_work_order_detail_result")]
public  		string
  queryWorkOrderDetailResult { get; set; }


}
}
