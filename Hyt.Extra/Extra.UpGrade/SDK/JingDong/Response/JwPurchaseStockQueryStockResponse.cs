using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class JwPurchaseStockQueryStockResponse : JdResponse{


         [XmlElement("querystock_result")]
public  		string
  querystockResult { get; set; }


}
}
