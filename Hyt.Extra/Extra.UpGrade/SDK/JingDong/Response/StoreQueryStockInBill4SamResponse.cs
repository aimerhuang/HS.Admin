using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class StoreQueryStockInBill4SamResponse : JdResponse{


         [XmlElement("query_stock_in_sam_result")]
public  		string
  queryStockInSamResult { get; set; }


}
}
