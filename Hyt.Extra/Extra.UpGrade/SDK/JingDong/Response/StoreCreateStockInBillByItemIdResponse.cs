using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class StoreCreateStockInBillByItemIdResponse : JdResponse{


         [XmlElement("stockin_sam_result")]
public  		string
  stockinSamResult { get; set; }


}
}
