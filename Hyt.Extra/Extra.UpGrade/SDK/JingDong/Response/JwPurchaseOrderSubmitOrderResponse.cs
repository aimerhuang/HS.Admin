using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class JwPurchaseOrderSubmitOrderResponse : JdResponse{


         [XmlElement("submitorder_result")]
public  		string
  submitorderResult { get; set; }


}
}
