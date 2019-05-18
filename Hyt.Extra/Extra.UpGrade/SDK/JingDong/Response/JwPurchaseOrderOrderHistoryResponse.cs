using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class JwPurchaseOrderOrderHistoryResponse : JdResponse{


         [XmlElement("orderhistory_result")]
public  		string
  orderhistoryResult { get; set; }


}
}
