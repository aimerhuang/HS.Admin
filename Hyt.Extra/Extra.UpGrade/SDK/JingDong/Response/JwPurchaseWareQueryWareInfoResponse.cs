using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class JwPurchaseWareQueryWareInfoResponse : JdResponse{


         [XmlElement("querywareinfo_result")]
public  		string
  querywareinfoResult { get; set; }


}
}
