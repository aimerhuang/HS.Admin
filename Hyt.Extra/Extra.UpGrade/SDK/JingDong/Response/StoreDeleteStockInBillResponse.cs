using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class StoreDeleteStockInBillResponse : JdResponse{


         [XmlElement("stock_in_delete_result")]
public  		string
  stockInDeleteResult { get; set; }


}
}
