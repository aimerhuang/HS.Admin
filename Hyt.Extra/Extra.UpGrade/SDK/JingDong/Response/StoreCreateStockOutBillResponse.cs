using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class StoreCreateStockOutBillResponse : JdResponse{


         [XmlElement("stockout_result")]
public  		string
  stockoutResult { get; set; }


}
}
