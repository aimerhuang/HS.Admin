using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EclpRtwQueryReceiptResponse : JdResponse{


         [XmlElement("queryReceipt_result")]
public  		string
  queryReceiptResult { get; set; }


}
}
