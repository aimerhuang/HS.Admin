using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class StoreQueryStockInBillResponse : JdResponse{


         [XmlElement("query_stock_in_result")]
public  		string
  queryStockInResult { get; set; }


}
}
