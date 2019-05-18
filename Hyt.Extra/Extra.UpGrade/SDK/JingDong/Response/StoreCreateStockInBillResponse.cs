using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class StoreCreateStockInBillResponse : JdResponse{


         [XmlElement("stockin_result")]
public  		string
  stockinResult { get; set; }


}
}
