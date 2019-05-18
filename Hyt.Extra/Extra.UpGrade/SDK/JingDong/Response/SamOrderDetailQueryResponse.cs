using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SamOrderDetailQueryResponse : JdResponse{


         [XmlElement("queryorderdetail_result")]
public  		string
  queryorderdetailResult { get; set; }


}
}
