using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class StoreQueryStockOutBillResponse : JdResponse{


         [XmlElement("query_stock_out_result")]
public  		string
  queryStockOutResult { get; set; }


}
}
